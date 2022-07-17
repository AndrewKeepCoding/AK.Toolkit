using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace AK.Toolkit.WinUI3.Localization;

public interface ILocalizer
{
    StringResources? GetLanguageResources(string language);

    IEnumerable<string> GetAvailableLanguages();

    void Initalize(string resourcesFolderPath, string resourcesFileName, string defaultLanguage);

    string GetCurrentLanguage();

    bool TrySetCurrentLanguage(string language);

    void RegisterRootElement(FrameworkElement rootElement);

    void RunLocalizationOnRegisteredRootElements(string? language = null);

    void RunLocalization(FrameworkElement rootElement, string? language = null);

    string? GetLocalizedString(string key, string? language = null);
}