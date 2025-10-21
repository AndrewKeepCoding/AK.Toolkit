using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public sealed partial class DataGridsPage : Page
{
    public DataGridsPage()
    {
        InitializeComponent();
        Loaded += DataGrids_Loaded;
    }

    public ObservableCollection<string> EmptyList { get; } = [];

    public ObservableCollection<string> SmallList { get; } = [];

    public ObservableCollection<string> LargeList { get; } = [];

    private void DataGrids_Loaded(object sender, RoutedEventArgs e)
    {
        SmallList.Add(Guid.NewGuid().ToString());

        for (int i = 0; i < 100; i++)
        {
            LargeList.Add(Guid.NewGuid().ToString());
        }
    }

    private void AddItemButton_Click(object sender, RoutedEventArgs e)
    {
        string newItem = Guid.NewGuid().ToString();
        EmptyList.Add(newItem);
        SmallList.Add(newItem);
        LargeList.Add(newItem);
    }

    private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
    {
        if (EmptyList.Count > 0)
        {
            EmptyList.RemoveAt(EmptyList.Count - 1);
        }

        if (SmallList.Count > 0)
        {
            SmallList.RemoveAt(SmallList.Count - 1);
        }

        if (LargeList.Count > 0)
        {
            LargeList.RemoveAt(LargeList.Count - 1);
        }
    }
}