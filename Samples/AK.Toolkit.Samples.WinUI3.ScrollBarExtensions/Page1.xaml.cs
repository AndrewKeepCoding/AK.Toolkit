using System;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public sealed partial class Page1 : Page
{
    public Page1()
    {
        InitializeComponent();

        for (int i = 0; i < 100; i++)
        {
            Items.Add(Guid.NewGuid().ToString());
        }
    }

    public ObservableCollection<string> Items { get; } = new();
}