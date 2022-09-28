using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using static AK.Toolkit.WinUI3.GridIndexer.GI;

namespace AK.Toolkit.WinUI3.GridIndexer;

public static class GridIndexer
{
    public static void RunGridIndexer(UIElement parent)
    {
        UpdateUIElementIndex(parent, IndexTarget.Row);
        UpdateUIElementIndex(parent, IndexTarget.Column);
    }

    private static void UpdateUIElementIndex(UIElement parent, IndexTarget indexTarget)
    {
        if (parent is Grid grid)
        {
            UpdateGridIndex(grid, indexTarget);
        }
        else if (parent is Panel panel)
        {
            foreach (UIElement child in panel.Children)
            {
                UpdateUIElementIndex(child, indexTarget);
            }
        }
        else if (parent is ContentControl contentControl &&
            contentControl.Content is UIElement content)
        {
            UpdateUIElementIndex(content, indexTarget);
        }
    }

    private static void UpdateGridIndex(Grid grid, IndexTarget indexTarget)
    {
        int index = 0;

        foreach (UIElement child in grid.Children)
        {
            if (child is FrameworkElement frameworkElement)
            {
                if (GI.GetTypeAndValue(frameworkElement, indexTarget) is (GI.ValueType ValueType, int Value))
                {
                    index = ValueType switch
                    {
                        GI.ValueType.Absolute => Value,
                        GI.ValueType.Relative => index + Value,
                        _ => throw new GridIndexerException($"{ValueType} is not implemented."),
                    };

                    int additionalCount = (index + 1) - grid.GetDefinitionsCount(indexTarget);

                    for (int i = 0; i < additionalCount; i++)
                    {
                        grid.AddDefinition(indexTarget);
                    }

                    frameworkElement.SetIndex(indexTarget, index);
                }

                UpdateUIElementIndex(child, indexTarget);
            }
        }
    }

    public static int GetDefinitionsCount(this Grid grid, IndexTarget indexTarget)
    {
        return indexTarget switch
        {
            IndexTarget.Row => grid.RowDefinitions.Count,
            IndexTarget.Column => grid.ColumnDefinitions.Count,
            _ => throw new GridIndexerException($"{indexTarget} is not implemented."),
        };
    }

    private static void AddDefinition(this Grid grid, IndexTarget indexTarget)
    {
        if (indexTarget is IndexTarget.Row)
        {
            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = grid.RowDefinitions.LastOrDefault() is RowDefinition previous
                    ? previous.Height
                    : new GridLength(0, GridUnitType.Auto)
            });
        }
        else if (indexTarget is IndexTarget.Column)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = grid.ColumnDefinitions.LastOrDefault() is ColumnDefinition previous
                    ? previous.Width
                    : new GridLength(0, GridUnitType.Auto)
            });
        }
    }

    private static void SetIndex(this FrameworkElement element, IndexTarget indexTarget, int index)
    {
        if (indexTarget is IndexTarget.Row)
        {
            Grid.SetRow(element, index);
        }
        else if (indexTarget is IndexTarget.Column)
        {
            Grid.SetColumn(element, index);
        }
    }
}