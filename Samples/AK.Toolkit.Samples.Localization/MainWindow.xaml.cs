using AK.Toolkit.WinUI3.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AK.Toolkit.Samples.Localization;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(this.AppTitleBar);

        this.NavigationViewControl.Loaded += NavigationViewControl_Loaded;

        if (Content is FrameworkElement content)
        {
            Localizer.Get().RegisterRootElement(content);
        }
    }

    private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.NavigationViewControl.SettingsItem is NavigationViewItem settingsItem)
        {
            Localizer.SetUid(settingsItem, "Settings");
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected is true)
        {
            _ = this.ContentFrame.Navigate(typeof(SettingsPage));
        }
        else if (args.SelectedItem is NavigationViewItem item &&
            Type.GetType("AK.Toolkit.Samples.Localization." + (string)item.Tag) is Type pageType)
        {
            _ = this.ContentFrame.Navigate(pageType);
        }
    }
}