using AK.Toolkit.WinUI3.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.Samples.Localization;

public sealed partial class MainWindow : Window
{
    private readonly ILocalizer _localizer;

    //public IEnumerable<Tuple<string, string>> AvailableLanguages { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        _localizer = Ioc.Default.GetRequiredService<ILocalizer>();

        if (Content is FrameworkElement content)
        {
            _localizer.RegisterRootElement(content);
        }

        //AvailableLanguages = _localizer.GetAvailableLanguages()
        //    .Select(x =>
        //    {
        //        string displayName = x;

        //        if (_localizer.GetLocalizedString(x) is string localizedDisplayName)
        //        {
        //            displayName = localizedDisplayName;
        //        }

        //        return new Tuple<string, string>(displayName, x);
        //    });
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

    //private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.AddedItems.Count > 0 &&
    //        (e.AddedItems[0] as Tuple<string, string>)?.Item2 is string language)
    //    {
    //        _localizer.RunLocalizationOnRegisteredRootElements(language);
    //    }
    //}
}