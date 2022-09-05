using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AK.Toolkit.WinUI3.Localization;

public partial class Localizer : DependencyObject, ILocalizer
{
    private readonly Dictionary<string, StringResourceListDictionary> _languageResources = new();

    private readonly HashSet<UIElement> _rootElements = new();

    private Localizer()
    {
    }

    private static ILocalizer Instance { get; set; } = EmptyLocalizer.Instance;

    private string CurrentLanguage { get; set; } = "en-US";

    public static ILocalizer Create(string resourcesFolderPath, string resourcesFileName = "Resources.resw", string defaultLanguage = "en-US")
    {
        Localizer localizer = new();
        localizer.Initalize(resourcesFolderPath, resourcesFileName, defaultLanguage);
        Instance = localizer;
        return Instance;
    }

    /// <summary>
    /// Get an instance of Localizer
    /// </summary>
    /// <returns></returns>
    public static ILocalizer Get() => Instance;

    /// <summary>
    /// Change Language at Runtitme without need to run `RunLocalizationOnRegisteredRootElements` method again.
    /// </summary>
    /// <param name="language">en-US</param>
    /// <returns></returns>
    public static bool SetCurrentLanguage(string language)
    {
        bool result = Instance.TrySetCurrentLanguage(language);
        Instance.RunLocalizationOnRegisteredRootElements();
        return result;
    }

    public IEnumerable<string> GetAvailableLanguages() => _languageResources.Keys;

    /// <summary>
    /// You need to Initialize Window with 2 parameters
    /// </summary>
    /// <param name="Root">Grid/StackPanel or any FrameworkElement that hosts elements</param>
    /// <param name="Content">Windows `Content` Properties</param>
    public void InitializeWindow(FrameworkElement Root, UIElement Content)
    {
        Instance.RunLocalization(Root);
        if (Content is FrameworkElement content)
        {
            Instance.RegisterRootElement(content);
        }
    }

    public string GetCurrentLanguage() => CurrentLanguage;

    public bool TrySetCurrentLanguage(string language)
    {
        if (GetAvailableLanguages().Contains(language) is true)
        {
            CurrentLanguage = language;
            RunLocalizationOnRegisteredRootElements();
            return true;
        }

        return false;
    }

    public void RegisterRootElement(FrameworkElement rootElement) => _rootElements.Add(rootElement);

    public void RunLocalizationOnRegisteredRootElements(string? language = null)
    {
        foreach (FrameworkElement rootElement in _rootElements)
        {
            RunLocalization(rootElement, language);
        }
    }

    public void RunLocalization(FrameworkElement rootElement, string? language = null)
    {
        language ??= CurrentLanguage;

        if (GetLanguageResources(language) is StringResourceListDictionary resourceListDictionary)
        {
            Localize(rootElement, resourceListDictionary);
        }
    }

    public StringResourceListDictionary? GetLanguageResources(string language)
    {
        if (_languageResources.TryGetValue(language, out StringResourceListDictionary? resourceListDictionary) is true)
        {
            return resourceListDictionary;
        }

        return null;
    }

    public string? GetLocalizedString(string key, string? language = null)
    {
        language ??= CurrentLanguage;

        if (_languageResources.TryGetValue(language, out StringResourceListDictionary? resourceListDictionary) is true &&
            resourceListDictionary.TryGetValue(key, out StringResourceList? resourceList) is true)
        {
            return resourceList.FirstOrDefault()?.Value;
        }

        return null;
    }

    private void Initalize(string resourcesFolderPath, string resourcesFileName = "Resources.resw", string defaultLanguage = "en-US")
    {
        CurrentLanguage = defaultLanguage;

        _languageResources.Clear();

        foreach (string folder in Directory.GetDirectories(resourcesFolderPath))
        {
            string resourceFilePath = Path.Combine(folder, resourcesFileName);

            if (LoadLanguageResources(resourceFilePath) is StringResourceListDictionary resourceListDictionary &&
                new DirectoryInfo(resourceFilePath).Parent?.Name is string languageCode)
            {
                _languageResources.Add(languageCode, resourceListDictionary);
            }
        }

        RegisterDefaultUIElementChildrenGetters();
    }

    private void Localize(UIElement element, StringResourceListDictionary resourceListDictionary)
    {
        if (GetUid(element) is string uid &&
            resourceListDictionary.TryGetValue(uid, out StringResourceList? resourceList) is true)
        {
            foreach (StringResource resource in resourceList)
            {
                if (GetDependencyProperty(element, resource.DependencyPropertyName) is DependencyProperty dependencyProperty)
                {
                    element.SetValue(dependencyProperty, resource.Value);
                }
            }
        }

        if (_childrenGetters.TryGetValue(element.GetType(), out var childrenGetter) is true &&
            childrenGetter is not null)
        {
            foreach (UIElement child in childrenGetter(element).Union(element.GetChildren()))
            {
                Localize(child, resourceListDictionary);
            }
        }
    }
}