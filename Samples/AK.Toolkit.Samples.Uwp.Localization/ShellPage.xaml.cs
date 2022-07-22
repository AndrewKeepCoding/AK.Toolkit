using AK.Toolkit.Uwp.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Uwp.Localization
{
    public sealed partial class ShellPage : Page
    {
        private readonly ILocalizer _localizer;

        public ShellPage()
        {
            this.InitializeComponent();
            this.Loaded += ShellPage_Loaded;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(AppTitleBar);

            _localizer = Ioc.Default.GetRequiredService<ILocalizer>();

             if (Content is FrameworkElement content)
            {
                _localizer.RegisterRootElement(content);
            }

            NavigationView.SelectedItem = NavigationView.MenuItems[0];
        }

        private void ShellPage_Loaded(object sender, RoutedEventArgs e)
        {
            _localizer.RunLocalization(NavigationView);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected is true)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItem is NavigationViewItem item &&
                     Type.GetType("AK.Toolkit.Samples.Uwp.Localization." + (string)item.Tag) is Type pageType)
            {
                ContentFrame.Navigate(pageType);
            }
        }
    }
}