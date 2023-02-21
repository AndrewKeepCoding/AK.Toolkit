using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.ScrollBarExtensions;

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