using AK.Toolkit.Utilities;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using static AK.Toolkit.Utilities.RandomStringGenerator;

namespace AK.Toolkit.Samples;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        UpdateDemoSuggestions();
    }

    private List<string> DemoSuggestions { get; } = new();

    private void UpdateDemoSuggestions()
    {
        DemoSuggestions.Clear();

        if (int.TryParse(RandomDemoSuggestionsCount.Text, out int suggestionsCount) is true)
        {
            for (int i = 0; i < suggestionsCount; i++)
            {
                string suggestion = RandomStringGenerator.GenerateString(OutputType.AlphaNumerics, 3, 10);
                DemoSuggestions.Add(suggestion);
            }
        }

        string[]? additionals = AdditionalSuggestions.Text.Split('\u002C');
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