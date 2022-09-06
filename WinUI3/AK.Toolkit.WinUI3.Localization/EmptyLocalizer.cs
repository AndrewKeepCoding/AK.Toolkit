using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3.Localization;

public class EmptyLocalizer : ILocalizer
{
    public static readonly ILocalizer Instance = new EmptyLocalizer();

    public void InitializeWindow(FrameworkElement Root, UIElement Content)
    {
    }

    public IEnumerable<string> GetAvailableLanguages() => Enumerable.Empty<string>();

    public string GetCurrentLanguage() => string.Empty;
    
    public string? GetLocalizedString(string key, string? language = null) => null;

    public StringResourceListDictionary? GetLanguageResources(string language) => null;

    public bool TrySetCurrentLanguage(string language) => false;

    public void RegisterRootElement(FrameworkElement rootElement)
    {
    }

    public void RunLocalizationOnRegisteredRootElements(string? language = null)
    {
    }

    public void RunLocalization(FrameworkElement rootElement, string? language = null)
    {
    }

    public bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func) => false;
}