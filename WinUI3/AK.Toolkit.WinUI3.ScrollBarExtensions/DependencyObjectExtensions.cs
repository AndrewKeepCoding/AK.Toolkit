using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

public static class DependencyObjectExtensions
{
    public static IEnumerable<DependencyObject> FindChildren(this DependencyObject parent)
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);
            yield return child;

            foreach (DependencyObject grandChild in child.FindChildren())
            {
                yield return grandChild;
            }
        }
    }

    public static IEnumerable<T> FindChildrenOfType<T>(this DependencyObject parent) where T : DependencyObject
    {
        return parent
            .FindChildren()
            .OfType<T>();
    }

    public static FrameworkElement? FindChildOfName(this DependencyObject parent, string name)
    {
        return parent
            .FindChildrenOfType<FrameworkElement>()
            .FirstOrDefault(x => x.Name == name);
    }
}