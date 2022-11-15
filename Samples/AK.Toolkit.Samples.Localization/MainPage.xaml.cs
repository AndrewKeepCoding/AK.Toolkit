using AK.Toolkit.WinUI3.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Localization;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        Localizer.Get().RunLocalization(this.Root);
    }
}