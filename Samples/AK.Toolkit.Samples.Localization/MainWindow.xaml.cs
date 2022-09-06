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
        SetTitleBar(AppTitleBar);

        if (Content is FrameworkElement content)
        {
            Localizer.Get().RegisterRootElement(content);
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected is true)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
        else if (args.SelectedItem is NavigationViewItem item &&
            Type.GetType("AK.Toolkit.Samples.Localization." + (string)item.Tag) is Type pageType)
        {
            ContentFrame.Navigate(pageType);
        }
    }
}