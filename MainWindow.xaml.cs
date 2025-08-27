using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

using MusicFileOrganizer.Services;
using MusicFileOrganizer.UI;
using MusicFileOrganizer.Utils;

using System;
using System.Diagnostics;

using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ConfirmDialog? confirmDialog;
        private PathFolderPicker? folderPicker;
        private readonly Organizer organizer = new Organizer();

        public MainWindow()
        {
            InitializeComponent();
            this.DispatcherQueue.TryEnqueue(() =>
            {
                folderPicker = new PathFolderPicker(this);
            });

        }

        private async void AddFolderClicked(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            StorageFolder srcFolder = await folderPicker.OpenFolderPicker();
            if (srcFolder is not null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("SourceFolderToken", srcFolder);
                IsTopButtonsEnabled(false);
                sw = Stopwatch.StartNew();
                organizer.SetSrcPathAsync(srcFolder.Path);
                await TableView.AddItemsAsync(await organizer.CreateFileMetaAsync());
                IsTopButtonsEnabled(true);
                sw.Stop();
                Debug.WriteLine($"파일 추가 실행 시간: {sw.ElapsedMilliseconds} ms, {sw.ElapsedMilliseconds / 1000} sec");
            }
        }
        private async void DestinationSelectClicked(object sender, RoutedEventArgs e)
        {
            StorageFolder dstFolder = await folderPicker.OpenFolderPicker();
            if (dstFolder is not null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("DestinationFolder", dstFolder);
                DstBar.DstPath = organizer.DstPath = dstFolder.Path;
            }
        }

        private async void OrganizeClicked(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            var fileRepo = organizer.FileMetaRepository;
            string dstPath = organizer.DstPath;

            if (!string.IsNullOrEmpty(dstPath))
            {
                IsTopButtonsEnabled(false);
                string content = StringUtil.CreateFolderTreePreview(fileRepo.CreateArtistAlbumPair());

                confirmDialog = new ConfirmDialog(this.Content.XamlRoot);
                var result = await confirmDialog.Get(content).ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    sw = Stopwatch.StartNew();
                    await organizer.Start();
                    CompleteNotification();
                    sw.Stop();
                    Debug.WriteLine($"파일 정리 실행 시간: {sw.ElapsedMilliseconds} ms, {sw.ElapsedMilliseconds / 1000} sec");
                }
                IsTopButtonsEnabled(true);
            }


        }
        private async void TabbedDstBar(object sender, RoutedEventArgs e)
        {
            string? dstPath = organizer.DstPath;

            if (string.IsNullOrEmpty(dstPath))
            {
                return;
            }
            var contentDialog = ContentDialogHelper.DstBarTapped(this.Content.XamlRoot);
            ContentDialogResult result = await contentDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(dstPath);
                await Launcher.LaunchFolderAsync(folder);
            }
        }

        private void IsTopButtonsEnabled(bool isEnabled)
        {
            CmdBar.IsAddFolderEnabled = isEnabled;
            CmdBar.IsOrganizeEnabled = isEnabled;
        }

        private void CompleteNotification()
        {
            AppNotification notification = new AppNotificationBuilder()
                .AddText("File Organizing Complete")
                .AddText("")
                .BuildNotification();
            AppNotificationManager.Default.Show(notification);

        }

        private void OnItemClick(object sender, Microsoft.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            // Handle item click event here
            // For example, you can show a message or perform an action
            var clickedItem = e.ClickedItem;
            if (clickedItem != null)
            {
                Debug.WriteLine($"Item clicked: {clickedItem}");
            }
        }

        private void TableViewClicked(object sender, RoutedEventArgs e)
        {

        }

    }
}