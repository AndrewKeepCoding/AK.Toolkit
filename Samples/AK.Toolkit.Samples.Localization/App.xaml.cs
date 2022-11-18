using AK.Toolkit.WinUI3.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace AK.Toolkit.Samples.Localization;

public partial class App : Application
{
    private readonly IHost host;

    private Window? window;

    public App()
    {
        InitializeComponent();
        this.host = BuildHost();
        Ioc.Default.ConfigureServices(this.host.Services);
        Localizer.Set(this.host.Services.GetRequiredService<ILocalizer>());
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.window = Ioc.Default.GetRequiredService<MainWindow>();
        this.window.Activate();
    }

    private static void InitializeLocalizer()
    {
        ILocalizer localizer = new LocalizerBuilder()
            // For a packaged app:
            //.AddResourcesStringsFolder(new LocalizerResourcesStringsFolder(@"C:/Projects/Strings"))
            // For a non-packaged app:
            .AddDefaultResourcesStringsFolder()
            .AddLanguageDictionaries(
                new List<LanguageDictionary>()
                {
                    new LanguageDictionary("ja")
                    {
                        new StringResource("ToggleSwitchHeader", "HeaderProperty", "トグルスイッチ"),
                        new StringResource("NavigationViewAutoSuggestBox", "PlaceholderProperty", "検索"),
                    }
                })
            .Build();
        Localizer.Set(localizer);
    }

    private static IHost BuildHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                _ = services
                    .AddSingleton<MainWindow>()
                    .UseLocalizer(options =>
                    {
                        options.AddDefaultResourcesStringsFolder = false;

                        options.AdditionalResourcesStringsFolders.Add(
                            new LocalizerResourcesStringsFolder(
                                StringsFolderPath: @"C:\Projects\Strings",
                                ResourcesFileName: @"Resources.resw"));

                        options.AdditionalLanguageDictionaries.Add(
                            new LanguageDictionary("en-US")
                            {
                                new StringResource("ToggleSwitchHeader", "HeaderProperty", "ToggleSwitch?"),
                                new StringResource("NavigationView.AutoSuggestBox", "PlaceholderTextProperty", "Search"),
                                new StringResource("Settings", "ContentProperty", "Settings"),
                            });
                        options.AdditionalLanguageDictionaries.Add(
                            new LanguageDictionary("ja")
                            {
                                new StringResource("ToggleSwitchHeader", "HeaderProperty", "トグルスイッチ"),
                                new StringResource("NavigationView.AutoSuggestBox", "PlaceholderTextProperty", "検索"),
                                new StringResource("Settings", "ContentProperty", "設定"),
                            });

                        options.DefaultLanguage = "ja";
                    });
            })
            .Build();
    }
}