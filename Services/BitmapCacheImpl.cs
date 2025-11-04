using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace MusicFileOrganizer.Utils
{
    public class BitmapCacheImpl
    {
        // 캐시에는 리사이즈된(썸네일) 바이트[]만 저장
        private readonly Dictionary<string, byte[]> _cache = new();

        public byte[]? Get(string key)
        {
            if (_cache.TryGetValue(key, out var imageData))
            {
                return imageData;
            }
            return null;
        }

        public void Add(string key, byte[] imageData)
        {
            if (key == null) return;
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = imageData;
            }
        }

        public void Clear()
        {
            _cache.Clear();
        }

        // key으로 캐시 존재하면 반환, 없고 imageData가 주어지면 리사이즈하여 캐시에 추가 후 반환
        public async Task<byte[]?> GetNAddAsync(string key, byte[]? imageData, uint thumbWidth = 32, uint thumbHeight = 32)
        {
            if (key == null) return null;

            if (_cache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            if (imageData == null) return null;

            try
            {
                var thumb = await CreateThumbnailAsync(imageData, thumbWidth, thumbHeight).ConfigureAwait(false);
                if (thumb != null)
                {
                    // 한 번에 쓰기(작은 바이트 배열)
                    lock (_cache)
                    {
                        if (!_cache.ContainsKey(key))
                        {
                            _cache[key] = thumb;
                        }
                    }

                    return _cache[key];
                }
            }
            catch
            {
                // 실패 시 원본을 그대로 저장하지 않고 null 반환
            }

            return null;
        }

        public int Count => _cache.Count;

        // InMemoryRandomAccessStream과 BitmapDecoder/Encoder를 사용해 썸네일을 생성
        private static async Task<byte[]?> CreateThumbnailAsync(byte[] sourceBytes, uint width, uint height)
        {
            if (sourceBytes == null || sourceBytes.Length == 0) return null;

            try
            {
                // 쓰기 가능한 메모리 스트림에 원본 이미지 작성
                using var input = new InMemoryRandomAccessStream();
                await input.WriteAsync(sourceBytes.AsBuffer()).AsTask().ConfigureAwait(false);
                input.Seek(0);

                var decoder = await BitmapDecoder.CreateAsync(input).AsTask().ConfigureAwait(false);

                var transform = new BitmapTransform
                {
                    ScaledWidth = width,
                    ScaledHeight = height,
                    InterpolationMode = BitmapInterpolationMode.Fant
                };

                // 픽셀 데이터 추출 (리사이즈 적용)
                var pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage).AsTask().ConfigureAwait(false);

                var pixels = pixelData.DetachPixelData();

                // PNG로 인코딩
                using var output = new InMemoryRandomAccessStream();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, output).AsTask().ConfigureAwait(false);

                // DPI를 그대로 따르되 픽셀 크기는 리사이즈된 값 사용
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                    width, height, decoder.DpiX, decoder.DpiY, pixels);

                await encoder.FlushAsync().AsTask().ConfigureAwait(false);

                output.Seek(0);
                var size = (uint)output.Size;

                var reader = new DataReader(output.GetInputStreamAt(0));
                await reader.LoadAsync(size).AsTask().ConfigureAwait(false);
                var result = new byte[size];
                reader.ReadBytes(result);
                return result;
            }
            catch
            {
                return null;
            }
        }
    }

    public static class BitmapCache
    {
        private static readonly BitmapCacheImpl _instance = new BitmapCacheImpl();

        // 비동기 버전으로 노출
        public static Task<byte[]?> GetNAddAsync(string key, byte[]? bytes, uint w = 32, uint h = 32)
            => _instance.GetNAddAsync(key, bytes, w, h);

        // 하위 호환을 위한 동기적 Get
        public static byte[]? Get(string key) => _instance.Get(key);
    }
}
