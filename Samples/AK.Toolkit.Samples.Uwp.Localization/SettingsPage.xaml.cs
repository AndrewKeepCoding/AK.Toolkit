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
        private readonly ILocalizer localizer;

        public SettingsPage()
        {
            InitializeComponent();
            this.localizer = Ioc.Default.GetRequiredService<ILocalizer>();
            this.localizer.RegisterRootElement(this.Root);

            AvailableLanguages = this.localizer.GetAvailableLanguages()
                .Select(x =>
                {
                    string displayName = x;

                    if (this.localizer.GetLocalizedStrings(x).FirstOrDefault() is string localizedDisplayName)
                    {
                        displayName = localizedDisplayName;
                    }

                    return new Tuple<string, string>(displayName, x);
                });

            Tuple<string, string> currentLanguage = AvailableLanguages
                .FirstOrDefault(x => x.Item2 == this.localizer.GetCurrentLanguage());

            if (currentLanguage != null)
            {
                this.LanguageRadioButtons.SelectedIndex = AvailableLanguages
                    .ToList()
                    .IndexOf(currentLanguage);
            }
        }

        public IEnumerable<Tuple<string, string>> AvailableLanguages { get; set; }

        public Tuple<string, string> CurrentLanguage { get; set; }

        private void LanguageRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 &&
                (e.AddedItems[0] as Tuple<string, string>)?.Item2 is string language)
            {
                this.localizer.SetLanguage(language);
            }
        }
    }
}