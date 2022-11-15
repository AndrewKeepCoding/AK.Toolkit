using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.Uwp.Localization
{
    public class StringResource
    {
        public StringResource(string key, string dependencyPropertyName, string value)
        {
            Key = key;
            DependencyPropertyName = dependencyPropertyName;
            Value = value;
        }

        public string Key { get; }

        public string DependencyPropertyName { get; }

        public string Value { get; }
    }

    public class LanguageDictionary : List<StringResource>
    {
        public LanguageDictionary(string languageKey)
        {
            LanguageKey = languageKey;
        }

        public string LanguageKey { get; private set; }
    }

    internal class LanguageDictionaries
    {
        private readonly Dictionary<string, LanguageDictionary> dictionaries = new Dictionary<string, LanguageDictionary>();

        public IEnumerable<string> AvailableLanguages => this.dictionaries.Keys;

        public IDictionary<string, LanguageDictionary> GetDictionaries() => this.dictionaries;

        public void AppendDictionary(LanguageDictionary languageDictionary)
        {
            if (this.dictionaries.TryGetValue(languageDictionary.LanguageKey, out LanguageDictionary dictionary) is false)
            {
                dictionary = new LanguageDictionary(languageDictionary.LanguageKey);
                this.dictionaries.Add(languageDictionary.LanguageKey, dictionary);
            }

            foreach (StringResource stringResource in languageDictionary.Select(x => x))
            {
                dictionary.Add(stringResource);
            }
        }

        public bool TryAddDictionary(string language, LanguageDictionary languageDictionary)
        {
            return this.dictionaries.TryAdd(language, languageDictionary);
        }

        public bool TryGetDictionary(string language, out LanguageDictionary languageDictionary)
        {
            return this.dictionaries.TryGetValue(language, out languageDictionary);
        }
    }
}