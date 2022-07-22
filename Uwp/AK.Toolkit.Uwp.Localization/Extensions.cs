using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AK.Toolkit.Uwp.Localization
{
    public static class Extensions
    {
        public static IEnumerable<UIElement> GetChildren(this UIElement parent, params Func<UIElement, bool>[] filters)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                if (VisualTreeHelper.GetChild(parent, i) is UIElement child)
                {
                    if (filters.All(filter => filter(child) is true))
                    {
                        yield return child;
                    }
                }
            }
        }

        public static IEnumerable<Type> GetHierarchyFromUIElement(this Type element)
        {
            if (element.GetTypeInfo().IsSubclassOf(typeof(UIElement)) != true)
            {
                yield break;
            }

            Type current = element;

            while (current != null && current != typeof(UIElement))
            {
                yield return current;
                current = current.GetTypeInfo().BaseType;
            }
        }
    }
}