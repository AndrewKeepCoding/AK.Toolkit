using AK.Toolkit.Uwp.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Uwp.Localization
{
    public sealed partial class MainPage : Page
    {
        private readonly ILocalizer localizer;

        public MainPage()
        {
            InitializeComponent();
            this.localizer = Ioc.Default.GetRequiredService<ILocalizer>();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.localizer.RunLocalization(this.Root);
        }
    }
}