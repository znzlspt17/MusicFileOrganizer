using MusicFileOrganizer.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Dispatching;

namespace MusicFileOrganizer.ViewModel
{
    public class TableViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FileMeta> MusicFiles { get; } = new();
        public async Task LoadFilesAsync(IAsyncEnumerable<FileMeta> items)
        {
            var _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            await foreach (var item in items)
            {
                if (_dispatcherQueue.HasThreadAccess)
                {
                    MusicFiles.Add(item);
                }
                else
                {
                    _dispatcherQueue.TryEnqueue(() => MusicFiles.Add(item));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}