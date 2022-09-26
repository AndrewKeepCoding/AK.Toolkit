using Microsoft.UI.Xaml.Controls;

namespace AK.Toolkit.Samples.GridIndexer;

public sealed partial class UsualGridPage : Page
{
    public UsualGridPage()
    {
        this.InitializeComponent();
    }

    private string SampleCode { get; } =
@"
<Grid
    ColumnDefinitions=""*,*,*,*,*""
    RowDefinitions=""*,*,*,*,*"">
    <TextBlock Grid.Row=""0"" Grid.Column=""0"" />
    <TextBlock Grid.Row=""1"" Grid.Column=""1"" />
    <TextBlock Grid.Row=""2"" Grid.Column=""2"" />
    <TextBlock Grid.Row=""3"" Grid.Column=""3"" />
    <TextBlock Grid.Row=""4"" Grid.Column=""4"" />
</Grid>
";
}
