using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

using MusicFileOrganizer.DTO;
using MusicFileOrganizer.Services;
using MusicFileOrganizer.UI;
using MusicFileOrganizer.Utils;
using MusicFileOrganizer.ViewModel;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using TagLib.Ape;

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
        public TableViewModel TableViewModel { get; } = new TableViewModel();
        private ObservableCollection<FileMeta> _musicFiles { get; } = new ObservableCollection<FileMeta>();
        private ConfirmDialog? _confirmDialog;
        private PathFolderPicker? _folderPicker;
        private readonly Organizer _organizer;

        public MainWindow()
        {
            _organizer = App.Services.GetService<Organizer>()!;

            InitializeComponent();

            this.DispatcherQueue.TryEnqueue(() =>
            {
                _folderPicker = new PathFolderPicker(this);
            });
            RootGrid.DataContext = TableViewModel;
        }

        private async void AddFolderClicked(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            StorageFolder srcFolder = await _folderPicker!.OpenFolderPicker();
            if (srcFolder is not null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("SourceFolderToken", srcFolder);
                IsTopButtonsEnabled(false);
                sw = Stopwatch.StartNew();
                _organizer.SetSrcPath(srcFolder.Path);
                await TableViewModel.LoadFilesAsync(await _organizer.CreateFileMetaAsync());
                IsTopButtonsEnabled(true);
                sw.Stop();
                Debug.WriteLine($"파일 추가 실행 시간: {sw.ElapsedMilliseconds} ms, {sw.ElapsedMilliseconds / 1000} sec");
            }
        }

        private async void DestinationSelectClicked(object sender, RoutedEventArgs e)
        {
            StorageFolder dstFolder = await _folderPicker!.OpenFolderPicker();
            if (dstFolder is not null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("DestinationFolder", dstFolder);
                DstBar.DstPath = _organizer.DstPath = dstFolder.Path;
            }
        }

        private async void OrganizeClicked(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            var fileRepo = _organizer.FileMetaRepository;
            string dstPath = _organizer.DstPath;

            if (!string.IsNullOrEmpty(dstPath))
            {
                IsTopButtonsEnabled(false);
                string content = StringUtil.CreateFolderTreePreview(fileRepo.CreateArtistAlbumPair());

                _confirmDialog = new ConfirmDialog(this.Content.XamlRoot);
                var result = await _confirmDialog.Get(content).ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    sw = Stopwatch.StartNew();
                    await _organizer.Start();
                    CompleteNotification();
                    sw.Stop();
                    Debug.WriteLine($"파일 정리 실행 시간: {sw.ElapsedMilliseconds} ms, {sw.ElapsedMilliseconds / 1000} sec");
                }
                IsTopButtonsEnabled(true);
            }
        }

        private async void TabbedDstBar(object sender, RoutedEventArgs e)
        {
            string? dstPath = _organizer.DstPath;

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


    }
}