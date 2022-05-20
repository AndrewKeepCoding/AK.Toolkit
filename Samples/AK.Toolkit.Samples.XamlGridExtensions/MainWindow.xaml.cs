using Microsoft.UI.Xaml;

namespace AK.Toolkit.Samples.XamlGridExtensions;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.RunGridIndexer();
    }
}