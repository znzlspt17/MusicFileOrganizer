using System;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;

namespace MusicFileOrganizer.UI
{
    class PathFolderPicker
    {
        private readonly FolderPicker folderPicker;
        private readonly IntPtr hwnd;

        public PathFolderPicker(Object target)
        {
            folderPicker = new FolderPicker();
            hwnd = WinRT.Interop.WindowNative.GetWindowHandle(target);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            folderPicker.FileTypeFilter.Add("*");
        }
        public async Task<StorageFolder> OpenFolderPicker()
        {
            return await folderPicker.PickSingleFolderAsync()!;
        }
    }
}
