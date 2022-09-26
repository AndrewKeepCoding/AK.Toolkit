using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.GridIndexer;

public sealed partial class GridIndexerPage : Page
{
    public GridIndexerPage()
    {
        this.InitializeComponent();
        WinUI3.GridIndexer.GridIndexer.RunGridIndexer(this.Content);
    }

    private string SampleCode { get; } =
@"
<Grid
    ColumnDefinitions=""*""
    RowDefinitions=""*"">
    <TextBlock GI.Row=""0"" GI.Column=""0"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
</Grid>
";
}