using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AK.Toolkit.Uwp.Localization
{
    public static class Extensions
    {
        public static IServiceCollection UseLocalizer(this IServiceCollection services, Action<LocalizerOptions> options = null)
        {
            return services.AddSingleton(factory =>
            {
                LocalizerOptions localizerOptions = new LocalizerOptions();
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