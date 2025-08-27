using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using MusicFileOrganizer.DTO;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer.UI
{

    public sealed partial class TableViewControl : UserControl
    {
        public ObservableCollection<FileMeta> Items
        {
            get => (ObservableCollection<FileMeta>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items),
                typeof(ObservableCollection<FileMeta>),
                typeof(TableViewControl),
                new PropertyMetadata(null)
            );

        public TableViewControl()
        {
            InitializeComponent();
        }
    }
}
