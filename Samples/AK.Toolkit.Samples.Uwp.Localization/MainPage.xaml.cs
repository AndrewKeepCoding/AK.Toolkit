using AK.Toolkit.Uwp.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Uwp.Localization
{
    public sealed partial class MainPage : Page
    {
        private readonly ILocalizer _localizer;

        public MainPage()
        {
            InitializeComponent();
            _localizer = Ioc.Default.GetRequiredService<ILocalizer>();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _localizer.RunLocalization(Root);
        }
    }
}