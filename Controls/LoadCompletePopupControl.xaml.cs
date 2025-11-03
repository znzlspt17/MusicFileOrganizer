using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MusicFileOrganizer.UI
{
    public sealed partial class LoadCompletePopupControl : UserControl
    {
        public LoadCompletePopupControl()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler CloseClicked;
        private int loadedFileCount = 0;
        private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
        {
            if (!StandardPopup.IsOpen)
            {
                PopupText.Text = loadedFileCount + " Loaded";
                StandardPopup.IsOpen = true;
            }
        }

        // Handles the Click event on the Button inside the Popup control and closes the Popup.
        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            if (StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = false;
            }
        }
    }
}
