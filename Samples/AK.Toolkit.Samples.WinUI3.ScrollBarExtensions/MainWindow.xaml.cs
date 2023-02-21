using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _ = this.ContentFrame.Navigate(typeof(Page1));
    }

    private void Page1Button_Click(object sender, RoutedEventArgs e)
    {
        _ = this.ContentFrame.Navigate(typeof(Page1));
    }

    private void Page2Button_Click(object sender, RoutedEventArgs e)
    {
        _ = this.ContentFrame.Navigate(typeof(Page2));
    }
}