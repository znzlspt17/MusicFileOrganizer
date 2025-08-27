using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using MusicFileOrganizer.Helper;

namespace MusicFileOrganizer.Helper
{
    public class ContentDialogBuilder
    {
        private readonly ContentDialog contentDialog;

        public ContentDialogBuilder()
        {
            contentDialog = new ContentDialog();
        }

        public ContentDialog Build(string title, string content, string primaryText, string closeText, XamlRoot xamlRoot)
        {
            contentDialog.Title = title;
            contentDialog.Content = content;
            contentDialog.PrimaryButtonText = primaryText;
            contentDialog.CloseButtonText = closeText;
            contentDialog.DefaultButton = ContentDialogButton.Close;
            contentDialog.XamlRoot = xamlRoot;

            return contentDialog;
        }
    }

}

public static class ContentDialogHelper
{
    private static readonly ContentDialogBuilder _impl = new ContentDialogBuilder();

    public static ContentDialog DstBarTapped(XamlRoot xamlRoot)
        => _impl.Build("Alarm", "Open The Destination Folder?", "Open", "Cancel", xamlRoot);


}
