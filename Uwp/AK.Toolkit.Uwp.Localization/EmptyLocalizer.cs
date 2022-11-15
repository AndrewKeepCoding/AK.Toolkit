using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace AK.Toolkit.Uwp.Localization
{
    public class EmptyLocalizer : ILocalizer
    {
        public static readonly ILocalizer Instance = new EmptyLocalizer();

        public IEnumerable<string> GetAvailableLanguages() => Enumerable.Empty<string>();

        public string GetCurrentLanguage() => string.Empty;

        public IEnumerable<string> GetLocalizedStrings(string key) => Enumerable.Empty<string>();

        public void SetLanguage(string language)
        {
        }

        public void RegisterRootElement(FrameworkElement rootElement, bool runLocalization = true)
        {
        }

        public void RunLocalizationOnRegisteredRootElements()
        {
        }

        public void RunLocalization(FrameworkElement rootElement)
        {
        }

        public bool TryGetLanguageDictionary(string language, out LanguageDictionary languageDictionary)
        {
            languageDictionary = null;
            return false;
        }

        public bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func) => false;
    }
}