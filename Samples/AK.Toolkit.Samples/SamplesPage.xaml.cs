using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace AK.Toolkit.Samples;

public sealed partial class SamplesPage : Page
{
    public SamplesPage()
    {
        InitializeComponent();
        UpdateDemoSuggestions();
    }

    private List<string> DemoSuggestions { get; } = new();

    private void UpdateDemoSuggestions()
    {
        DemoSuggestions.Clear();

        string[]? additionals = this.AdditionalSuggestions.Text.Split('\u002C');
        Random random = new();

        foreach (string item in additionals)
        {
            int index = random.Next(0, DemoSuggestions.Count);
            DemoSuggestions.Insert(index, item);
        }
    }

    private void UpdateDemoSuggestionsButton_Click(object sender, RoutedEventArgs e)
    {
        UpdateDemoSuggestions();
    }
}