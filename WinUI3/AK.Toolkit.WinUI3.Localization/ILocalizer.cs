using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace AK.Toolkit.WinUI3.Localization;

public interface ILocalizer
{
    StringResourceListDictionary? GetLanguageResources(string language);

    IEnumerable<string> GetAvailableLanguages();

    string GetCurrentLanguage();

    bool TrySetCurrentLanguage(string language);

    void RegisterRootElement(FrameworkElement rootElement);

    void RunLocalizationOnRegisteredRootElements(string? language = null);

    void RunLocalization(FrameworkElement rootElement, string? language = null);

    string? GetLocalizedString(string key, string? language = null);

    bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func);
}