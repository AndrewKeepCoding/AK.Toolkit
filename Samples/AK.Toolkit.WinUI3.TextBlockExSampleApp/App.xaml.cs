using Microsoft.UI.Xaml;

namespace AK.Toolkit.WinUI3.TextBlockExSampleApp;
public partial class App : Application
{
    private Window? window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.window = new MainWindow();
        this.window.Activate();
    }
}
