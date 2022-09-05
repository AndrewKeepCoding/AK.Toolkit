using AK.Toolkit.WinUI3.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Localization;

public sealed partial class MainPage : Page
{
    //private readonly ILocalizer _localizer;

    public MainPage()
    {
        InitializeComponent();
        //_localizer = Ioc.Default.GetRequiredService<ILocalizer>();
        Loaded += MainPage_Loaded;
    }

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        //_localizer.RunLocalization(Root);
        Localizer.Get().RunLocalization(Root);
    }
}