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
        private readonly ILocalizer localizer;

        public ShellPage()
        {
            InitializeComponent();
            Loaded += ShellPage_Loaded;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(this.AppTitleBar);

            this.localizer = Ioc.Default.GetRequiredService<ILocalizer>();

            if (Content is FrameworkElement content)
            {
                this.localizer.RegisterRootElement(content);
            }

            this.NavigationView.SelectedItem = this.NavigationView.MenuItems[0];
        }

        private void ShellPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.localizer.RunLocalization(this.NavigationView);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected is true)
            {
                _ = this.ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItem is NavigationViewItem item &&
                     Type.GetType("AK.Toolkit.Samples.Uwp.Localization." + (string)item.Tag) is Type pageType)
            {
                _ = this.ContentFrame.Navigate(pageType);
            }
        }
    }
}