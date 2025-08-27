using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer.UI
{
    public sealed partial class DestinationSelectorControl : UserControl
    {
        public event RoutedEventHandler DestinationSelectClicked;
        public event RoutedEventHandler DestinationTapped;


        public DestinationSelectorControl()
        {
            InitializeComponent();
        }

        public string DstPath
        {
            get => DestinationPathBox.Text;
            set => DestinationPathBox.Text = value;
        }

        private void OnClickDestination(object sender, RoutedEventArgs e)
        {
            DestinationSelectClicked?.Invoke(this, e);
        }
        private void OnTappedDestination(object sender, RoutedEventArgs e)
        {
            DestinationTapped?.Invoke(this, e);
        }
    }

}
