using MusicFileOrganizer.DTO;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Services
{
    public class FileMetaRepository
    {
        private IList<FileMeta> items;

        public FileMetaRepository()
        {
            items = new List<FileMeta>();
        }

        public void Add(FileMeta file)
        {
            items.Add(file);
        }

        public async Task<IList<FileMeta>> AddAll(IAsyncEnumerable<FileMeta> files)
        {
            await foreach (var file in files)
            {
                items.Add(file);

            }
            return items;
        }

        public void Remove(FileMeta file)
        {
            items.Remove(file);
        }

        public void Clear()
        {
            items.Clear();
        }

        public IList<FileMeta> GetAll()
        {
            return items;
        }

        public FileMeta? FindByName(string name)
        {
            foreach (FileMeta file in items)
            {
                if (file.Name == name)
                {
                    return file;
                }
            }
            return null;
        }

        public Dictionary<string, List<string>> CreateArtistAlbumPair()
        {
            Dictionary<string, List<string>> pairs = new Dictionary<string, List<string>>();

            foreach (FileMeta file in items)
            {
                string artist = file.Artist ?? "Unknown Artist";
                string album = file.AlbumTitle ?? "Unknown Album";

                if (!pairs.ContainsKey(artist))
                {
                    pairs[artist] = new List<string>();
                }

                if (!pairs[artist].Contains(album))
                {
                    pairs[artist].Add(album);
                }
            }
            return pairs;
        }
    }
}
