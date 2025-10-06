using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

internal static class DependencyObjectExtensions
{
    internal static IEnumerable<DependencyObject> FindDescendants(this DependencyObject element)
    {
        int childCount = VisualTreeHelper.GetChildrenCount(element);

        for (int i = 0; i < childCount; i++)
        {
            if (VisualTreeHelper.GetChild(element, i) is not DependencyObject child)
            {
                continue;
            }

            yield return child;

            foreach (DependencyObject childOfChild in child.FindDescendants())
            {
                yield return childOfChild;
            }
        }
    }

    internal static FrameworkElement? FindDescendant(this DependencyObject element, string name)
    {
        foreach (FrameworkElement descendant in element.FindDescendants().OfType<FrameworkElement>())
        {
            if (descendant.Name == name)
            {
                return descendant;
            }
        }

        return null;
    }
}
