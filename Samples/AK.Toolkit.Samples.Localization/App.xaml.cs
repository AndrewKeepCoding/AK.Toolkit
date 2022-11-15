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

        // For non-packaged app:
        //string resourcesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Strings");

        // For packaged app:
        //string resourcesFolderPath = @"C:\\Projects\\Strings";

        //_ = Localizer.Create(resourcesFolderPath);
        //var localizer = Ioc.Default.GetRequiredService<ILocalizer>();
        InitializeLocalizer();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
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
                    }
                })
            .Build();
        Localizer.Set(localizer);
    }

    private static IHost BuildHost()
    {
        return Host.CreateDefaultBuilder()
            //.UseLocalizer(options =>
            //{
            //    options.AddDefaultResourcesStringsFolder = false;

            //    options.AdditionalResourcesStringsFolders.Add(
            //        new LocalizerResourcesStringsFolder(
            //            StringsFolderPath: @"C:\Projects\Strings",
            //            ResourcesFileName: @"Resources.resw"));

            //    options.DefaultLanguage = "ja";
            //})
            .ConfigureServices((context, services) =>
            {
                _ = services.AddSingleton<MainWindow>();
            })
            .Build();
    }
}