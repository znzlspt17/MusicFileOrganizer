using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer.UI
{
    public sealed partial class CommandBarControl : UserControl
    {
        public event RoutedEventHandler AddFolderClicked;
        public event RoutedEventHandler OrganizeClicked;

        public CommandBarControl()
        {
            InitializeComponent();
        }
        public bool IsAddFolderEnabled
        {
            get => BtnAdd.IsEnabled;
            set => BtnAdd.IsEnabled = value;
        }
        public bool IsOrganizeEnabled
        {
            get => BtnOgnz.IsEnabled;
            set => BtnOgnz.IsEnabled = value;
        }


        private void OnClickAdd(object sender, RoutedEventArgs e)
        {
            AddFolderClicked?.Invoke(this, e);
        }

        private void OnClickOrganize(object sender, RoutedEventArgs e)
        {
            OrganizeClicked?.Invoke(this, e);
        }
    }
}
