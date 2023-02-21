using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public partial class App : Application
{
    private Window? window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.window = new MainWindow();
        this.window.Activate();
    }
}