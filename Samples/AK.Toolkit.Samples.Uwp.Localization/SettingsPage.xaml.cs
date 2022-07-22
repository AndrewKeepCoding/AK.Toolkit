using AK.Toolkit.Uwp.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.Uwp.Localization
{
    public sealed partial class SettingsPage : Page
    {
        private readonly ILocalizer _localizer;

        public SettingsPage()
        {
            this.InitializeComponent();
            _localizer = Ioc.Default.GetRequiredService<ILocalizer>();
            _localizer.RegisterRootElement(Root);

            AvailableLanguages = _localizer.GetAvailableLanguages()
                .Select(x =>
                {
                    string displayName = x;

                    if (_localizer.GetLocalizedString(x) is string localizedDisplayName)
                    {
                        displayName = localizedDisplayName;
                    }

                    return new Tuple<string, string>(displayName, x);
                });

            var currentLanguage = AvailableLanguages.Where(x => x.Item2 == _localizer.GetCurrentLanguage()).FirstOrDefault();

            if (currentLanguage != null)
            {
                LanguageRadioButtons.SelectedIndex = AvailableLanguages.ToList().IndexOf(currentLanguage);
            }
        }

        public IEnumerable<Tuple<string, string>> AvailableLanguages { get; set; }

        public Tuple<string, string> CurrentLanguage { get; set; }

        private void LanguageRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 &&
                (e.AddedItems[0] as Tuple<string, string>)?.Item2 is string language &&
                _localizer.TrySetCurrentLanguage(language) is true)
            {
                _localizer.RunLocalizationOnRegisteredRootElements();
            }
        }
    }
}