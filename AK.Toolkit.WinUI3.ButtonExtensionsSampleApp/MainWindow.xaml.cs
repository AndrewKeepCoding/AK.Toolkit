using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.WinUI3.ButtonExtensionsSampleApp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        button.Content = "Clicked";
    }
}
