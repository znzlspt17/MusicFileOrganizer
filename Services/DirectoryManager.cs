using MusicFileOrganizer.DTO;
using MusicFileOrganizer.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Services
{
    public class DirectoryManager
    {
        private string? _srcPath = string.Empty;
        private string? _dstPath = string.Empty;
        private string? fullPath = string.Empty;
        private string? left = string.Empty;
        private string? right = string.Empty;
        private Dictionary<(string, string), (string, string)> cachedFolder = new Dictionary<(string, string), (string, string)>();
        public string? SrcPath
        {
            get => _srcPath;
            set
            {
                _srcPath = value;
            }
        }
        public string? DstPath
        {
            get => _dstPath;
            set
            {
                _dstPath = value;
            }
        }

        public DirectoryManager() { }

        public async Task<DirectoryInfo> CreateFolderAsync(FileMeta file)
        {
            DirectoryInfo directoryInfo;

            left = StringUtil.IsNullOrEmpty(file.Artist);
            right = StringUtil.IsNullOrEmpty(file.AlbumTitle);

            var isExist = TrySkipExistFolder((left, right), out var cached);
            left = cached.cachedLeft;
            right = cached.cachedRight;
            fullPath = string.Intern(Path.Combine(_dstPath!, left!, right!));

            try
            {
                directoryInfo = new DirectoryInfo(fullPath);
                if (!isExist)
                {
                    await Task.Run(() => Directory.CreateDirectory(directoryInfo.FullName));
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating directory: {ex.Message}");
                throw;
            }

            return directoryInfo;
        }

        public async void DeleteCreatedFolderOnCancel()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_dstPath);
            directoryInfo.Delete();
        }

        private bool TrySkipExistFolder((string keyleft, string keyright) tree, out (string cachedLeft, string cachedRight) value)
        {
            var key = tree;
            bool isExist = cachedFolder.TryGetValue((key.keyleft, key.keyright), out (string cachedLeft, string cachedRight) existingFolder);
            if (isExist)
            {
                value = existingFolder;
                return true;
            }
            else
            {
                left = StringUtil.InvalidDirCharCleaner(left!.ToCharArray());
                right = StringUtil.InvalidDirCharCleaner(right!.ToCharArray());
                cachedFolder.Add(key, (left!, right!));
                value = (left!, right!);
                return false;
            }
        }

    }
}
