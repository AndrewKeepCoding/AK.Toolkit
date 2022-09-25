using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.GridExtensions;

public sealed partial class GridIndexerPage : Page
{
    public GridIndexerPage()
    {
        this.InitializeComponent();
        this.RunGridIndexer();
    }

    private string SampleCode { get; } =
@"
<Grid>
    <TextBlock GI.Row=""0"" GI.Column=""0"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
    <TextBlock GI.Row=""+1"" GI.Column=""+1"" />
</Grid>
";
}