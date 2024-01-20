using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AK.Toolkit.WinUI3.TextBlockExSampleApp;

public sealed partial class ShellPage : Page
{
    public ShellPage()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            _ = this.HighlightingAutoSuggestBox.Focus(FocusState.Programmatic);
        };
    }

    private void NavigationView_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not NavigationView navigationView ||
            Resources.TryGetValue("HighlightingTextContentTemplate", out object resource) is false ||
            resource is not DataTemplate navigationViewItemContentTemplate)
        {
            return;
        }

        IReadOnlyList<NavigationViewItem> navigationViewItems = navigationView.GetAllNavigationViewItems().ToList();

        if (navigationViewItems.Count > 0)
        {
            navigationView.SelectedItem = navigationViewItems[0];
        }

        foreach (NavigationViewItem navigationViewItem in navigationViewItems)
        {
            navigationViewItem.Loaded += (_, _) =>
            {
                if (navigationViewItem.FindDescendant<ContentPresenter>(x => x.Name is "ContentPresenter") is not ContentPresenter contentPresenter)
                {
                    return;
                }

                contentPresenter.ContentTemplate = navigationViewItemContentTemplate;
            };
        }
    }

    private void SetNavigationViewItemsContentTemplate(NavigationView navigationView)
    {
        if (Resources.TryGetValue("HighlightingTextContentTemplate", out object resource) is false ||
            resource is not DataTemplate highlightingTextContentTemplate)
        {
            return;
        }

        IReadOnlyList<NavigationViewItem> navigationViewItems = navigationView.GetAllNavigationViewItems().ToList();

        foreach (NavigationViewItem navigationViewItem in navigationViewItems)
        {
            if (navigationViewItem.FindDescendant<ContentPresenter>(x => x.Name is "ContentPresenter") is not ContentPresenter contentPresenter)
            {
                continue;
            }

            contentPresenter.ContentTemplate = highlightingTextContentTemplate;
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem navigationViewItem ||
            navigationViewItem.Tag is not string tag)
        {
            return;
        }

        Type pageType = tag switch
        {
            "MainPage" => typeof(MainPage),
            "SubPage" => typeof(SubPage),
            _ => throw new InvalidOperationException(),
        };

        _ = this.ContentFrame.Navigate(pageType);
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is not TextHighlightablePage highligtablePage)
        {
            return;
        }

        highligtablePage.SetBinding(
            TextHighlightablePage.HighlightingTextProperty,
            new Binding
            {
                Source = this.HighlightingAutoSuggestBox,
                Path = new PropertyPath("Text"),
                Mode = BindingMode.TwoWay
            });
    }
}
