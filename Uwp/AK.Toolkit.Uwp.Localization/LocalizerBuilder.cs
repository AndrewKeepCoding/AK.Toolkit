using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AK.Toolkit.Uwp.Localization
{
    public class LocalizerBuilder
    {
        private readonly List<Action> actions = new List<Action>();

        private readonly LanguageDictionaries languageDictionaries = new LanguageDictionaries();

        private bool isBuilt;

        private string defaultLanguage;

        public LocalizerBuilder When(Func<bool> condition)
        {
            if (condition.Invoke() is false)
            {
                this.actions.RemoveAt(this.actions.Count - 1);
            }

            return this;
        }

        public LocalizerBuilder AddDefaultResourcesStringsFolder()
        {
            this.actions.Add(() =>
            {
                string stringsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Strings");
                LoadResourcesStringsFolder(new LocalizerResourcesStringsFolder(stringsFolderPath));
            });

            return this;
        }

        public LocalizerBuilder AddResourcesStringsFolder(LocalizerResourcesStringsFolder resourcesStringsFolder)
        {
            this.actions.Add(() =>
            {
                LoadResourcesStringsFolder(resourcesStringsFolder);
            });

            return this;
        }

        public LocalizerBuilder AddResourcesStringsFolders(IEnumerable<LocalizerResourcesStringsFolder> resourcesStringsFolders)
        {
            this.actions.Add(() =>
            {
                foreach (LocalizerResourcesStringsFolder stringsFolder in resourcesStringsFolders)
                {
                    LoadResourcesStringsFolder(stringsFolder);
                }
            });

            return this;
        }

        public LocalizerBuilder AddLanguageDictionary(LanguageDictionary languageDictionary)
        {
            this.actions.Add(() =>
            {
                this.languageDictionaries.AppendDictionary(languageDictionary);
            });

            return this;
        }

        public LocalizerBuilder AddLanguageDictionaries(IEnumerable<LanguageDictionary> languageDictionaries)
        {
            this.actions.Add(() =>
            {
                foreach (LanguageDictionary languageDictionary in languageDictionaries)
                {
                    this.languageDictionaries.AppendDictionary(languageDictionary);
                }
            });

            return this;
        }

        public LocalizerBuilder SetDefaultLanguage(string language)
        {
            this.defaultLanguage = language;
            return this;
        }

        public ILocalizer Build()
        {
            if (this.isBuilt is true)
            {
                throw new InvalidOperationException("This Localizer.Builder is already built.");
            }

            foreach (Action action in this.actions)
            {
                action.Invoke();
            }

            Localizer localizer = new Localizer();
            localizer.Initialize(this.languageDictionaries);

            if (string.IsNullOrEmpty(this.defaultLanguage) is false)
            {
                localizer.SetLanguage(this.defaultLanguage);
            }

            this.isBuilt = true;

            return localizer;
        }

        private static StringResource CreateStringResource(XmlNode xmlNode)
        {
            if (xmlNode?.Attributes?["name"]?.Value is string name &&
                (xmlNode?["value"]?.InnerText) is string value)
            {
                (string Key, string DependencyPropertyName) = GetKeyAndDependencyPropertyName(name);
                return new StringResource(Key, $"{DependencyPropertyName}Property", value);
            }

            return null;
        }

        private static (string Key, string Property) GetKeyAndDependencyPropertyName(string name)
        {
            return name.LastIndexOf(".") is int lastSeparatorIndex &&
                lastSeparatorIndex > 1
                    ? (name.Substring(0, lastSeparatorIndex), name.Substring(lastSeparatorIndex + 1))
                    : (name, string.Empty);
        }

        private void LoadResourcesStringsFolder(LocalizerResourcesStringsFolder resourcesStringsFolder)
        {
            foreach (string folder in Directory.GetDirectories(resourcesStringsFolder.StringsFolderPath))
            {
                string languageFilePath = Path.Combine(folder, resourcesStringsFolder.ResourcesFileName);
                DirectoryInfo directoryInfo = new DirectoryInfo(languageFilePath);

                if (directoryInfo.Parent?.Name is string languageKey &&
                    CreateLanguageDictionaryFromLanguageResources(
                        languageKey,
                        languageFilePath) is LanguageDictionary languageDictionary)
                {
                    this.languageDictionaries.AppendDictionary(languageDictionary);
                }
            }
        }

        private LanguageDictionary CreateLanguageDictionaryFromLanguageResources(string languageKey, string resourceFilePath)
        {
            LanguageDictionary languageDictionary = new LanguageDictionary(languageKey);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(resourceFilePath);

            if (xmlDoc.SelectNodes("//root/data") is XmlNodeList nodeList)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (CreateStringResource(node) is StringResource resource)
                    {
                        languageDictionary.Add(resource);
                    }
                }
            }

            return languageDictionary;
        }
    }
}