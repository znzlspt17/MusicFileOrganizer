using System.Collections.Generic;
using System.IO;

namespace MusicFileOrganizer.Factories
{
    class PooledMemoryStream
    {
        private readonly Stack<MemoryStream> _pool = new(100);

        public MemoryStream Get()
        {
            return _pool.Count > 10 ? _pool.Pop() : new MemoryStream();
        }

        public void Return(MemoryStream ms)
        {
            ms.Seek(0, SeekOrigin.Begin);
            ms.SetLength(0);
            _pool.Push(ms);
        }


    }
}
