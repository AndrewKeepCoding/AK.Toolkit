using AK.Toolkit.WinUI3.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.Localization;

public partial class App : Application
{
    private readonly IHost _host;

    private Window? _window;

    public App()
    {
        InitializeComponent();
        _host = BuildHost();
        Ioc.Default.ConfigureServices(_host.Services);
        // For non-packaged app:
        //string resourcesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Strings");

        // For packaged app:
        string resourcesFolderPath = @"C:\\Projects\\Strings";

        _ = Localizer.Create(resourcesFolderPath);
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = Ioc.Default.GetRequiredService<MainWindow>();
        _window.Activate();

        Localizer.Get().RunLocalizationOnRegisteredRootElements();
    }

    private static IHost BuildHost() => Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            _ = services
                .AddSingleton<MainWindow>()
                //.AddSingleton<ILocalizer, Localizer>((serviceProvider) =>
                //{
                //    string resourcesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Strings");
                //    Localizer localizer = new();
                //    localizer.Initalize(resourcesFolderPath);
                //    return localizer;
                //})
                ;
        })
        .Build();
}