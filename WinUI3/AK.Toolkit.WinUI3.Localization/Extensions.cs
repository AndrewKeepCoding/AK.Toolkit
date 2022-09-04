using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AK.Toolkit.WinUI3.Localization;

public static class Extensions
{
    /// <summary>
    /// You need to Initialize Window with 2 parameters
    /// </summary>
    /// <param name="Localizer"></param>
    /// <param name="Root">Grid/StackPanel or any FrameworkElement that hosts elements</param>
    /// <param name="Content">Windows `Content` Properties</param>
    public static void InitializeWindowEx(this Localizer Localizer, FrameworkElement Root, UIElement Content)
    {
        Localizer.RunLocalization(Root);
        if (Content is FrameworkElement content)
        {
            Localizer.RegisterRootElement(content);
        }
    }

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
        if (element.GetTypeInfo().IsSubclassOf(typeof(UIElement)) is not true)
        {
            yield break;
        }

        Type? current = element;

        while (current is not null && current != typeof(UIElement))
        {
            yield return current;
            current = current.GetTypeInfo().BaseType;
        }
    }
}