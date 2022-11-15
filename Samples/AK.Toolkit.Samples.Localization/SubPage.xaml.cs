using AK.Toolkit.WinUI3.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace AK.Toolkit.Samples.Localization;

public record Person(string FirstName, string LastName);

public sealed partial class SubPage : Page
{
    public List<Person> People = new();

    public SubPage()
    {
        InitializeComponent();
        Loaded += SubPage_Loaded;

        People.Add(new Person(FirstName: "Ted", LastName: "Mosby"));
        People.Add(new Person(FirstName: "Tracy", LastName: "McConnell"));
        People.Add(new Person(FirstName: "Marshall", LastName: "Eriksen"));
        People.Add(new Person(FirstName: "Lilly", LastName: "Aldrin"));
        People.Add(new Person(FirstName: "Barney", LastName: "Stinson"));
        People.Add(new Person(FirstName: "Robin", LastName: "Scherbatsky"));
    }

    private void SubPage_Loaded(object sender, RoutedEventArgs e)
    {
        Localizer.Get().RunLocalization(this.Root);
    }
}