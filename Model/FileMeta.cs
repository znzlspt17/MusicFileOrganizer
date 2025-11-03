using Microsoft.UI.Xaml.Media.Imaging;

using System.ComponentModel;

namespace MusicFileOrganizer.DTO
{
    public class FileMeta : INotifyPropertyChanged
    {
        private BitmapImage? _albumArt;
        private string? _path;
        private string? _name;
        private uint _track;
        private string? _title;
        private string? _albumTitle;
        private string? _artist;

        public BitmapImage? AlbumArt
        {
            get => _albumArt;
            set
            {
                _albumArt = value;
                OnPropertyChanged(nameof(AlbumArt));
            }
        }
        public string? Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public uint Track
        {
            get => _track;
            set
            {
                _track = value;
                OnPropertyChanged(nameof(Track));
            }
        }
        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string? AlbumTitle
        {
            get => _albumTitle;
            set
            {
                _albumTitle = value;
                OnPropertyChanged(nameof(AlbumTitle));
            }
        }

        public string? Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

