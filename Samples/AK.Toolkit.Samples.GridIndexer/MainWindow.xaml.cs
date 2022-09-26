using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.GridIndexer;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        WinUI3.GridIndexer.GridIndexer.RunGridIndexer(this.Content);
    }
}
