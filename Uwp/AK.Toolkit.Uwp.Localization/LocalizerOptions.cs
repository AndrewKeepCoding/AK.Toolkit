using System.Collections.Generic;

namespace AK.Toolkit.Uwp.Localization
{
    public class LocalizerOptions
    {
        public bool AddDefaultResourcesStringsFolder { get; set; } = true;

        public List<LocalizerResourcesStringsFolder> AdditionalResourcesStringsFolders { get; } = new List<LocalizerResourcesStringsFolder>();

        public List<LanguageDictionary> AdditionalLanguageDictionaries { get; } = new List<LanguageDictionary>();

        public string DefaultLanguage { get; set; } = "en-US";
    }
}