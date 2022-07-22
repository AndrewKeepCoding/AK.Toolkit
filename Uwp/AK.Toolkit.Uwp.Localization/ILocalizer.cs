using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace AK.Toolkit.Uwp.Localization
{
    public interface ILocalizer
    {
        StringResourceListDictionary GetLanguageResources(string language);

        IEnumerable<string> GetAvailableLanguages();

        void Initalize(string resourcesFolderPath, string resourcesFileName = "Resources.resw", string defaultLanguage = "en-US");

        string GetCurrentLanguage();

        bool TrySetCurrentLanguage(string language);

        void RegisterRootElement(FrameworkElement rootElement);

        void RunLocalizationOnRegisteredRootElements(string language = null);

        void RunLocalization(FrameworkElement rootElement, string language = null);

        string GetLocalizedString(string key, string language = null);

        bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func);
    }
}