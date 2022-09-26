using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.GridIndexer;

public partial class App : Application
{
    private Window window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        this.window = new MainWindow();
        this.window.Activate();
    }
}
