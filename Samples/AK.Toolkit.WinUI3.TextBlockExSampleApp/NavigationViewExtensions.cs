using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3.TextBlockExSampleApp;

public static class NavigationViewExtensions
{
    public static IEnumerable<NavigationViewItem> GetAllNavigationViewItems(this NavigationView navigationView)
    {
        foreach (NavigationViewItem item in navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .SelectMany(x => x.Flatten()))
        {
            yield return item;
        }

        if (navigationView.IsSettingsVisible is true &&
            navigationView.SettingsItem is NavigationViewItem settingsItem)
        {
            yield return settingsItem;
        }
    }

    public static IEnumerable<NavigationViewItem> Flatten(this NavigationViewItem sourceItem)
    {
        Stack itemsStack = new();
        itemsStack.Push(sourceItem);

        while (itemsStack.Count > 0)
        {
            if (itemsStack.Pop() is not NavigationViewItem currentItem)
            {
                continue;
            }

            yield return currentItem;

            foreach (NavigationViewItem childItem in currentItem.MenuItems.OfType<NavigationViewItem>())
            {
                itemsStack.Push(childItem);
            }
        }
    }
}
