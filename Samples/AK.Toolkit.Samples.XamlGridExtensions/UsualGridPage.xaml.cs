using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AK.Toolkit.Samples.GridExtensions
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsualGridPage : Page
    {
        public UsualGridPage()
        {
            this.InitializeComponent();
        }

        private string SampleCode { get; } =
@"
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width=""*"" />
        <ColumnDefinition Width=""*"" />
        <ColumnDefinition Width=""*"" />
        <ColumnDefinition Width=""*"" />
        <ColumnDefinition Width=""*"" />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
        <RowDefinition Height=""*"" />
        <RowDefinition Height=""*"" />
        <RowDefinition Height=""*"" />
        <RowDefinition Height=""*"" />
        <RowDefinition Height=""*"" />
    </Grid.RowDefinitions>
    <TextBlock Grid.Row=""0"" Grid.Column=""0"" />
    <TextBlock Grid.Row=""1"" Grid.Column=""1"" />
    <TextBlock Grid.Row=""2"" Grid.Column=""2"" />
    <TextBlock Grid.Row=""3"" Grid.Column=""3"" />
    <TextBlock Grid.Row=""4"" Grid.Column=""4"" />
</Grid>
";
    }
}