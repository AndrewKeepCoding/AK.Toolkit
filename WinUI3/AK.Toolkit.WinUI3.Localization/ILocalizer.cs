using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace AK.Toolkit.WinUI3.Localization;

public interface ILocalizer
{
    IEnumerable<string> GetAvailableLanguages();

    string GetCurrentLanguage();

    IEnumerable<string> GetLocalizedStrings(string key);

    void SetLanguage(string language);

    void RegisterRootElement(FrameworkElement rootElement, bool runLocalization = false);

    void RunLocalizationOnRegisteredRootElements();

    void RunLocalization(FrameworkElement rootElement);

    bool TryGetLanguageDictionary(string language, out LanguageDictionary? languageDictionary);

    bool TryRegisterUIElementChildrenGetters(Type type, Func<DependencyObject, IEnumerable<DependencyObject>> func);
}