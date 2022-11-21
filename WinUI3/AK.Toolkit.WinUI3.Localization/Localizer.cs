using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace AK.Toolkit.WinUI3.Localization;

public partial class Localizer : DependencyObject, ILocalizer
{
    private readonly HashSet<UIElement> rootElements = new();

    internal Localizer()
    {
    }

    internal static ILocalizer Instance { get; set; } = EmptyLocalizer.Instance;

    private LanguageDictionaries LanguageDictionaries { get; set; } = new();

    private string CurrentLanguage { get; set; } = "en-US";

    public static ILocalizer Get() => Instance;

    public static void Set(ILocalizer localizer) => Instance = localizer;

    public IEnumerable<string> GetAvailableLanguages() => LanguageDictionaries.AvailableLanguages;

    public string GetCurrentLanguage() => CurrentLanguage;

    public IEnumerable<string> GetLocalizedStrings(string key)
    {
        if (LanguageDictionaries.TryGetDictionary(
            CurrentLanguage,
            out LanguageDictionary? languageDictionary) is true &&
            languageDictionary is not null)
        {
            return languageDictionary
                .Where(x => x.Key == key)
                .Select(x => x.Value);
        }

        return Enumerable.Empty<string>();
    }

    public void SetLanguage(string language)
    {
        if (GetAvailableLanguages().Contains(language) is true)
        {
            CurrentLanguage = language;
            RunLocalizationOnRegisteredRootElements();
            return;
        }

        throw new NotImplementedException($"{language} is not available.");
    }

    public void RegisterRootElement(FrameworkElement rootElement, bool runLocalization = false)
    {
        rootElement.Loaded += RootElement_Loaded;
        rootElement.Unloaded += RootElement_Unloaded;

        _ = this.rootElements.Add(rootElement);

        if (runLocalization is true)
        {
            RunLocalization(rootElement);
        }
    }

    private void RootElement_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement rootElement)
        {
            RunLocalization(rootElement);
        }
    }

    private void RootElement_Unloaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement rootElement)
        {
            rootElement.Loaded -= RootElement_Loaded;
            rootElement.Unloaded -= RootElement_Unloaded;
            _ = this.rootElements.Remove(rootElement);
        }
    }

    public void RunLocalizationOnRegisteredRootElements()
    {
        foreach (FrameworkElement rootElement in this.rootElements.OfType<FrameworkElement>())
        {
            RunLocalization(rootElement);
        }
    }

    public void RunLocalization(FrameworkElement rootElement)
    {
        if (TryGetLanguageDictionary(
            CurrentLanguage,
            out LanguageDictionary? languageDictionary) is true &&
            languageDictionary is not null)
        {
            Localize(rootElement, languageDictionary);
        }
    }

    public bool TryGetLanguageDictionary(string language, out LanguageDictionary? languageDictionary)
    {
        return LanguageDictionaries.TryGetDictionary(language, out languageDictionary);
    }

    public bool TryRegisterUIElementChildrenGetters(Type type, Func<DependencyObject, IEnumerable<DependencyObject>> func)
    {
        return this.childrenGetters.TryAdd(type, func);
    }

    internal void Initialize(LanguageDictionaries languageDictionaries)
    {
        LanguageDictionaries = languageDictionaries;
        RegisterDefaultUIElementChildrenGetters();
    }

    private static DependencyProperty? GetDependencyProperty(DependencyObject element, string dependencyPropertyName)
    {
        Type type = element.GetType();

        if (type.GetProperty(
            dependencyPropertyName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is PropertyInfo propertyInfo &&
            propertyInfo.GetValue(null) is DependencyProperty property)
        {
            return property;
        }
        else if (type.GetField(
            dependencyPropertyName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is FieldInfo fieldInfo &&
            fieldInfo.GetValue(null) is DependencyProperty field)
        {
            return field;
        }

        return null;
    }

    private void Localize(DependencyObject element, LanguageDictionary languageDictionary)
    {
        if (GetUid(element) is string uid)
        {
            foreach (StringResource resource in languageDictionary.Where(x => x.Key == uid))
            {
                if (GetDependencyProperty(
                    element,
                    resource.DependencyPropertyName) is DependencyProperty dependencyProperty)
                {
                    element.SetValue(dependencyProperty, resource.Value);
                }
                else
                {
                    _ = TryLocalizeELementsWithoutDependencyProperty(element, resource.Value);
                }
            }
        }

        if (this.childrenGetters.TryGetValue(
            element.GetType(),
            out Func<DependencyObject, IEnumerable<DependencyObject>>? childrenGetter) is true &&
            childrenGetter is not null)
        {
            foreach (DependencyObject child in childrenGetter(element)
                .Union((element as UIElement)?.GetChildren() ?? Enumerable.Empty<DependencyObject>()))
            {
                Localize(child, languageDictionary);
            }
        }
    }

    private bool TryLocalizeELementsWithoutDependencyProperty(DependencyObject element, string value)
    {
        if (element is Run run)
        {
            run.Text = value;
            return true;
        }
        else if (element is Hyperlink hyperlink && hyperlink.Inlines.Count is 0)
        {
            hyperlink.Inlines.Add(new Run() { Text = value });
            return true;
        }

        return false;
    }
}