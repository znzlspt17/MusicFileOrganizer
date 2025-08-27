using Microsoft.UI.Xaml.Media.Imaging;

using MusicFileOrganizer.DTO;
using MusicFileOrganizer.Factories;
using MusicFileOrganizer.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Factories
{
    public class FileMetaFactoryImpl
    {

        private readonly PooledMemoryStream pool = new PooledMemoryStream();

        public FileMetaFactoryImpl()
        {

        }

        public async Task<FileMeta> CreateAsync(string srcPath, string name)
        {
            string fullPath = Path.Combine(srcPath, name);
            var ms = pool.Get();

            using var tagFile = TagLib.File.Create(fullPath);
            BitmapImage? albumArt = new BitmapImage
            {
                DecodePixelWidth = 32,
                DecodePixelHeight = 32
            };

            byte[]? albumArtBytes = AlbumArtCache(tagFile);
            if (albumArtBytes != null & albumArtBytes?.Length != 0)
            {
                try
                {
                    ms.Write(albumArtBytes);
                    ms.Seek(0, SeekOrigin.Begin);
                    await albumArt.SetSourceAsync(ms.AsRandomAccessStream());
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
                Path = fullPath,
                Name = name,
                Title = tagFile.Tag.Title,
                Track = tagFile.Tag.Track,
                AlbumTitle = tagFile.Tag.Album,
                Artist = tagFile.Tag.FirstAlbumArtist,
                AlbumArt = albumArt
            };
        }

        public async IAsyncEnumerable<FileMeta> CreateBulkAsync(string srcPath, IEnumerable<string> names)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            await foreach (var name in names.ToAsyncEnumerable())
            {
                FileMeta meta;
                try
                {
                    meta = await CreateAsync(srcPath, name);
                }
                catch (TagLib.UnsupportedFormatException)
                {
                    continue;
                }
                yield return meta;
            }
            Console.WriteLine($"스레드 실행 시간: {stopwatch.ElapsedMilliseconds} ms");
        }

        private byte[]? AlbumArtCache(TagLib.File? tagFile)
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
                return BitmapCache.GetNAdd(albumTitle, bytes);
            }
            return null;

        }
    }
}
public static class FileMetaFactory
{
    private static readonly FileMetaFactoryImpl _impl = new FileMetaFactoryImpl();

    public static Task<FileMeta> CreateAsync(string srcPath, string name)
        => _impl.CreateAsync(srcPath, name);

    public static IAsyncEnumerable<FileMeta> CreateBulkAsync(string srcPath, IEnumerable<string> names)
        => _impl.CreateBulkAsync(srcPath, names);
}

