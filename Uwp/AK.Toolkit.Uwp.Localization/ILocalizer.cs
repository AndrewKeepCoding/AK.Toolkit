using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace AK.Toolkit.Uwp.Localization
{
    public interface ILocalizer
    {
        IEnumerable<string> GetAvailableLanguages();

        string GetCurrentLanguage();

        IEnumerable<string> GetLocalizedStrings(string key);

        void SetLanguage(string language);

        void RegisterRootElement(FrameworkElement rootElement, bool runLocalization = true);

        void RunLocalizationOnRegisteredRootElements();

        void RunLocalization(FrameworkElement rootElement);

        bool TryGetLanguageDictionary(string language, out LanguageDictionary languageDictionary);

        bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func);
    }
}