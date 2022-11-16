using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AK.Toolkit.WinUI3.Localization;

public static class Extensions
{
    public static IServiceCollection UseLocalizer(this IServiceCollection services, Action<LocalizerOptions>? options = null)
    {
        return services.AddSingleton(factory =>
        {
            LocalizerOptions localizerOptions = new();
            options?.Invoke(localizerOptions);

            return new LocalizerBuilder()
                .AddDefaultResourcesStringsFolder()
                    .When(() => localizerOptions.AddDefaultResourcesStringsFolder is true)
                .AddResourcesStringsFolders(localizerOptions.AdditionalResourcesStringsFolders)
                .SetDefaultLanguage(localizerOptions.DefaultLanguage)
                .Build();
        });
    }

    internal static IEnumerable<UIElement> GetChildren(this UIElement parent)
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            if (VisualTreeHelper.GetChild(parent, i) is UIElement child)
            {
                yield return child;
            }
        }
    }

    internal static IEnumerable<Type> GetHierarchyFromUIElement(this Type element)
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