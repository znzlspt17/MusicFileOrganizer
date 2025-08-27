using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MusicFileOrganizer.UI
{
    class ConfirmDialog
    {
        private readonly XamlRoot _xamlRoot;
        private readonly ContentDialog dialog;
        private readonly TextBlock textBlock;
        private readonly StackPanel stackPanel;
        private readonly ScrollViewer scrollViewer;

        private const string dialogTitle = "Process Music File Organization?";

        public ConfirmDialog(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
            textBlock = new TextBlock();
            stackPanel = new StackPanel();
            scrollViewer = new ScrollViewer();
            dialog = new ContentDialog();
        }

        public ContentDialog Get(string content)
        {
            InitDialog().Content = scrollViewer;
            InitStackPanel().Children.Add(textBlock);
            InitScrollViewer().Content = stackPanel;
            InitTextBlock(content);

            return dialog;
        }

        private ContentDialog InitDialog()
        {
            dialog.XamlRoot = _xamlRoot;
            dialog.Title = dialogTitle;
            dialog.PrimaryButtonText = "Yes";
            dialog.CloseButtonText = "No";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.IsPrimaryButtonEnabled = true;
            dialog.IsSecondaryButtonEnabled = true;

            return dialog;
        }
        private ScrollViewer InitScrollViewer()
        {
            scrollViewer.XamlRoot = dialog.XamlRoot;
            scrollViewer.MaxHeight = 400;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            return scrollViewer;
        }

        private StackPanel InitStackPanel()
        {
            stackPanel.XamlRoot = scrollViewer.XamlRoot;
            return stackPanel;
        }

        private TextBlock InitTextBlock(string content)
        {
            textBlock.XamlRoot = stackPanel.XamlRoot;
            textBlock.Text = content;
            textBlock.Margin = new Thickness(0, 0, 0, 10);
            textBlock.FontWeight = FontWeights.Bold;

            return textBlock;
        }

    }
}
