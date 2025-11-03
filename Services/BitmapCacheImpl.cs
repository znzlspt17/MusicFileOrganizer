using System.Collections.Generic;

namespace MusicFileOrganizer.Utils
{
    public class BitmapCacheImpl
    {
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
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = imageData;
            }
        }
        public void Clear()
        {
            _cache.Clear();
        }

        public byte[]? GetNAdd(string key, byte[]? imageData)
        {
            if (key != null)
            {
                if (!_cache.ContainsKey(key))
                {
                    _cache[key] = imageData;
                }

                if (_cache.TryGetValue(key, out var bytes))
                {
                    return bytes;
                }
            }
            return null;
        }
        public int Count => _cache.Count;
    }

    public static class BitmapCache
    {
        private static readonly BitmapCacheImpl _instance = new BitmapCacheImpl();
        public static byte[]? GetNAdd(string key, byte[]? bytes) => _instance.GetNAdd(key, bytes);
    }
}
