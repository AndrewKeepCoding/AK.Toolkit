using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Linq;
using Microsoft.UI.Xaml.Data;
using System.Collections.Generic;

namespace AK.Toolkit.WinUI3.TextBlockExSampleApp;

public sealed partial class MainPage : TextHighlightablePage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ColorPicker colorPicker ||
            colorPicker.FindDescendants()
                .OfType<Grid>()
                .Where(x => x.Name is "RgbPanel" or "HsvPanel") is not IEnumerable<Grid> panels)
        {
            return;
        }

        foreach (Grid panel in panels)
        {
            foreach (TextBlock label in panel.Children.OfType<TextBlock>())
            {
                TextBlockEx textBlockEx = label.CreateTextBlockEx();

                textBlockEx.SetBinding(
                    TextBlockEx.HighlightingTextProperty,
                    new Binding
                    {
                        Source = this,
                        Path = new PropertyPath(nameof(TextBlockEx.HighlightingText)),
                        Mode = BindingMode.OneWay,
                    });

                label.Visibility = Visibility.Collapsed;
                panel.Children.Add(textBlockEx);
            }
        }
    }

    private void ComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ComboBox comboBox ||
            comboBox.FindDescendant<ContentPresenter>(x => x.Name is "ContentPresenter") is not ContentPresenter contentPresenter ||
            contentPresenter.Content is not TextBlock placeholderTextBlock)
        {
            return;
        }

        TextBlockEx textBlockEx = placeholderTextBlock.CreateTextBlockEx();
        textBlockEx.SetBinding(
            TextBlockEx.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(TextBlockEx.HighlightingText)),
                Mode = BindingMode.OneWay,
            });

        contentPresenter.Content = textBlockEx;
    }

    private void RatingControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not RatingControl ratingControl ||
            ratingControl.FindDescendant<TextBlock>(x => x.Name is "Caption") is not TextBlock caption ||
            caption.Parent is not StackPanel captionParent)
        {
            return;
        }

        TextBlockEx textBlockEx = caption.CreateTextBlockEx();
        textBlockEx.SetBinding(
            TextBlockEx.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(TextBlockEx.HighlightingText)),
                Mode = BindingMode.OneWay,
            });
        textBlockEx.VerticalAlignment = VerticalAlignment.Stretch;
        caption.Visibility = Visibility.Collapsed;
        captionParent.Children.Add(textBlockEx);
    }

    private void CalendarDatePicker_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not CalendarDatePicker calendarDatePicker ||
            calendarDatePicker.FindDescendant<TextBlock>(x => x.Name is "DateText") is not TextBlock dateText ||
            dateText.Parent is not Grid rootGrid)
        {
            return;
        }

        TextBlockEx textBlockEx = dateText.CreateTextBlockEx();
        textBlockEx.SetBinding(
            TextBlockEx.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(TextBlockEx.HighlightingText)),
                Mode = BindingMode.OneWay,
            });
        textBlockEx.SetBinding(
            TextBlockEx.TextProperty,
            new Binding
            {
                Source = dateText,
                Path = new PropertyPath(nameof(TextBlock.Text)),
                Mode = BindingMode.OneWay,
            });
        dateText.Visibility = Visibility.Collapsed;
        rootGrid.Children.Add(textBlockEx);
    }
}

public static class TextBlockExtensions
{
    public static TextBlockEx CreateTextBlockEx(this TextBlock textBlock)
    {
        TextBlockEx textBlockEx = new()
        {
            Text = textBlock.Text,
            Margin = textBlock.Margin,
            Padding = textBlock.Padding,
            Width = textBlock.Width,
            Height = textBlock.Height,
            FontFamily = textBlock.FontFamily,
            FontSize = textBlock.FontSize,
            FontWeight = textBlock.FontWeight,
            FontStyle = textBlock.FontStyle,
            Foreground = textBlock.Foreground,
            VerticalAlignment = textBlock.VerticalAlignment,
            HorizontalAlignment = textBlock.HorizontalAlignment,
            TextAlignment = textBlock.TextAlignment,
            TextWrapping = textBlock.TextWrapping,
            IsHitTestVisible = false,
        };

        Grid.SetRow(textBlockEx, Grid.GetRow(textBlock));
        Grid.SetColumn(textBlockEx, Grid.GetColumn(textBlock));
        Grid.SetRowSpan(textBlockEx, Grid.GetRowSpan(textBlock));
        Grid.SetColumnSpan(textBlockEx, Grid.GetColumnSpan(textBlock));

        return textBlockEx;
    }
}
