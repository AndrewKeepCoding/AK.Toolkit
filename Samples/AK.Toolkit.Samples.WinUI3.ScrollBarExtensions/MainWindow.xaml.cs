using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.NavigationViewControl.SelectedItem = this.NavigationViewControl
            .MenuItems.FirstOrDefault(x => x is NavigationViewItem { Content: "Annotations" });

        this.IgnoreAllKeepExpandedPropertyChangedEventsSwitch.IsOn = AK.Toolkit.WinUI3.ScrollBarExtensions.IgnoreAllKeepExpandedPropertyChangedEvents;
        this.EnableKeepExpandedDebugLoggingSwitch.IsOn = AK.Toolkit.WinUI3.ScrollBarExtensions.EnableKeepExpandedDebugLogging;
    }

    private void IgnoreAllKeepExpandedPropertyChangedEventsSwitch_Toggled(object sender, RoutedEventArgs _)
    {
        if (sender is ToggleSwitch { IsOn: bool isOn })
        {
            AK.Toolkit.WinUI3.ScrollBarExtensions.IgnoreAllKeepExpandedPropertyChangedEvents = isOn;
        }
    }

    private void EnableKeepExpandedDebugLoggingSwitch_Toggled(object sender, RoutedEventArgs _)
    {
        if (sender is ToggleSwitch { IsOn: bool isOn })
        {
            AK.Toolkit.WinUI3.ScrollBarExtensions.EnableKeepExpandedDebugLogging = isOn;
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected is true)
        {
        }
        else if (args.SelectedItem is NavigationViewItem item &&
            item.Content is string content &&
            Type.GetType($"AK.Toolkit.Samples.WinUI3.ScrollBarExtensions.{content}Page") is Type pageType)
        {
            _ = this.ContentFrame.Navigate(pageType);
        }
    }
}