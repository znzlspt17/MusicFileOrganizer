using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Dispatching;
using Windows.Storage.Streams;

using MusicFileOrganizer.DTO;
using MusicFileOrganizer.Factories;
using MusicFileOrganizer.Utils;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Factories
{
    public class FileMetaFactoryImpl
    {
        private readonly ConcurrentDictionary<string, Lazy<string>> _cache = new();
        private readonly PooledMemoryStream pool = new PooledMemoryStream();
        private DispatcherQueue? _dispatcher;

        public FileMetaFactoryImpl(DispatcherQueue? dispatcher = null)
        {
            _dispatcher = dispatcher;
        }

        public void SetDispatcher(DispatcherQueue dispatcher) => _dispatcher = dispatcher;

        public async Task<FileMeta> CreateAsync(string srcPath, string relPath, string name)
        {
            string dirPath = Path.Combine(srcPath, relPath);
            string fileFullPath = Path.Combine(dirPath, name);

            
            var ms = pool.Get();

            using var tagFile = TagLib.File.Create(fileFullPath);
            BitmapImage? albumArt = new BitmapImage
            {
                DecodePixelWidth = 32,
                DecodePixelHeight = 32
            };

            byte[]? albumArtBytes = await AlbumArtCacheAsync(tagFile).ConfigureAwait(false);
            if (albumArtBytes != null && albumArtBytes.Length != 0)
            {
                try
                {
                    ms.Position = 0;
                    ms.SetLength(0);
                    ms.Write(albumArtBytes, 0, albumArtBytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);

                    // UI 스레드에서 SetSourceAsync 호출 (주입된 DispatcherQueue 사용)
                    await SetBitmapImageSourceOnUiThreadAsync(albumArt, ms.AsRandomAccessStream()).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting album art source: {ex.Message} file : {name}");
                    albumArt = null;
                }
            }
            else
            {
                albumArt = null;
            }

            pool.Return(ms);

            return new FileMeta
            {
                Path = dirPath,
                Name = name,
                Title = tagFile.Tag.Title,
                Track = tagFile.Tag.Track,
                AlbumTitle = tagFile.Tag.Album,
                Artist = tagFile.Tag.FirstAlbumArtist,
                AlbumArt = albumArt
            };
        }

        public async IAsyncEnumerable<FileMeta> CreateBulkAsync(string srcPath, IAsyncEnumerable<(string,string)> tuples)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            string relpath = string.Empty;
            await foreach (var tuple in tuples)
            {
                var lazy = _cache.GetOrAdd(tuple.Item1,
                  relPath => new Lazy<string>(() => Path.GetRelativePath(srcPath, relPath), 
                                        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication));
                relpath = lazy.Value;
                FileMeta meta;
                try
                {
                    meta = await CreateAsync(srcPath, relpath, tuple.Item2);
                }
                catch (TagLib.UnsupportedFormatException)
                {
                    continue;
                }
                yield return meta;
            }
            Console.WriteLine($"스레드 실행 시간: {stopwatch.ElapsedMilliseconds} ms");
        }

        private async Task<byte[]?> AlbumArtCacheAsync(TagLib.File? tagFile)
        {
            var pictures = tagFile!.Tag.Pictures;
            var albumTitle = tagFile.Tag.Album;
            byte[]? bytes = null;
            if (pictures != null && pictures.Length > 0)
            {
                bytes = pictures[0].Data.Data;
            }

            if (albumTitle != null)
            {
                // 비동기 캐시 호출, 썸네일 크기는 32x32로 고정
                return await BitmapCache.GetNAddAsync(albumTitle, bytes, 32, 32).ConfigureAwait(false);
            }
            return null;
        }

        private Task SetBitmapImageSourceOnUiThreadAsync(BitmapImage image, IRandomAccessStream ras)
        {
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 주입된 디스패처 우선, 없으면 현재 스레드의 디스패처 시도
            var dispatcher = _dispatcher ?? DispatcherQueue.GetForCurrentThread();
            if (dispatcher == null)
            {
                tcs.SetException(new InvalidOperationException("DispatcherQueue is not available. Initialize the factory with the UI DispatcherQueue (e.g. MainWindow.DispatcherQueue)."));
                return tcs.Task;
            }

            bool queued = dispatcher.TryEnqueue(() =>
            {
                try
                {
                    // UI 스레드에서 SetSourceAsync 호출
                    var op = image.SetSourceAsync(ras);
                    op.AsTask().ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            tcs.TrySetException(t.Exception?.InnerException ?? t.Exception!);
                        else if (t.IsCanceled)
                            tcs.TrySetCanceled();
                        else
                            tcs.TrySetResult(null);
                    }, TaskScheduler.Default);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            if (!queued)
            {
                tcs.SetException(new InvalidOperationException("DispatcherQueue.TryEnqueue failed."));
            }

            return tcs.Task;
        }
    }
}
public static class FileMetaFactory
{
    private static readonly FileMetaFactoryImpl _impl = new FileMetaFactoryImpl();

    public static Task<FileMeta> CreateAsync(string srcPath, string relPath, string name)
        => _impl.CreateAsync(srcPath, relPath, name);

    public static IAsyncEnumerable<FileMeta> CreateBulkAsync(string srcPath, IAsyncEnumerable<(string,string)> tuples)
        => _impl.CreateBulkAsync(srcPath, tuples);

    // 앱 시작 시 UI 스레드의 DispatcherQueue를 전달하세요
    public static void SetDispatcherQueue(Microsoft.UI.Dispatching.DispatcherQueue dispatcher)
        => _impl.SetDispatcher(dispatcher);
}

