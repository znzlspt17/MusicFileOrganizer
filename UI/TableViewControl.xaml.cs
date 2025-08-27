using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using MusicFileOrganizer.DTO;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer.UI
{

    public sealed partial class TableViewControl : UserControl
    {

        public event RoutedEventHandler TableViewClicked;
        public ObservableCollection<FileMeta> Items { get; set; }

        public TableViewControl()
        {
            InitializeComponent();

            Items = new ObservableCollection<FileMeta>();
        }



        public async Task AddItemsAsync(IAsyncEnumerable<FileMeta> items)
        {
            await foreach (var item in items)
            {
                if (DispatcherQueue.HasThreadAccess)
                {
                    Items.Add(item);
                }
                else
                {
                    DispatcherQueue.TryEnqueue(() => Items.Add(item));
                }
            }
        }
    }
}
