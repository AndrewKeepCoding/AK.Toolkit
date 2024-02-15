using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using CommunityToolkit.WinUI;
using System.Linq;

namespace AK.Toolkit.WinUI3.RichTextBlockExtensionsSampleApp;

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
                RichTextBlock richTextBlock = label.CreateRichTextBlock();

                richTextBlock.SetBinding(
                    RichTextBlockExtensions.HighlightingTextProperty,
                    new Binding
                    {
                        Source = this,
                        Path = new PropertyPath("HighlightingText"),
                        Mode = BindingMode.OneWay,
                    });

                label.Visibility = Visibility.Collapsed;
                panel.Children.Add(richTextBlock);
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

        RichTextBlock richTextBloc = placeholderTextBlock.CreateRichTextBlock();
        richTextBloc.SetBinding(
            RichTextBlockExtensions.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath("HighlightingText"),
                Mode = BindingMode.OneWay,
            });

        contentPresenter.Content = richTextBloc;
    }

    private void RatingControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not RatingControl ratingControl ||
            ratingControl.FindDescendant<TextBlock>(x => x.Name is "Caption") is not TextBlock caption ||
            caption.Parent is not StackPanel captionParent)
        {
            return;
        }

        RichTextBlock richTextBlock = caption.CreateRichTextBlock();
        richTextBlock.SetBinding(
            RichTextBlockExtensions.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath("HighlightingText"),
                Mode = BindingMode.OneWay,
            });
        richTextBlock.VerticalAlignment = VerticalAlignment.Stretch;
        caption.Visibility = Visibility.Collapsed;
        captionParent.Children.Add(richTextBlock);
    }

    private void CalendarDatePicker_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not CalendarDatePicker calendarDatePicker ||
            calendarDatePicker.FindDescendant<TextBlock>(x => x.Name is "DateText") is not TextBlock dateText ||
            dateText.Parent is not Grid rootGrid)
        {
            return;
        }

        RichTextBlock richTextBlock = dateText.CreateRichTextBlock();
        richTextBlock.SetBinding(
            RichTextBlockExtensions.HighlightingTextProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath("HighlightingText"),
                Mode = BindingMode.OneWay,
            });
        richTextBlock.SetBinding(
            RichTextBlockExtensions.TextProperty,
            new Binding
            {
                Source = dateText,
                Path = new PropertyPath(nameof(TextBlock.Text)),
                Mode = BindingMode.OneWay,
            });
        dateText.Visibility = Visibility.Collapsed;
        rootGrid.Children.Add(richTextBlock);
    }
}

public static class RichTextBlocktensions
{
    public static RichTextBlock CreateRichTextBlock(this TextBlock textBlock)
    {
        RichTextBlock richTextBlock = new()
        {
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

        RichTextBlockExtensions.SetText(richTextBlock, textBlock.Text);

        Grid.SetRow(richTextBlock, Grid.GetRow(textBlock));
        Grid.SetColumn(richTextBlock, Grid.GetColumn(textBlock));
        Grid.SetRowSpan(richTextBlock, Grid.GetRowSpan(textBlock));
        Grid.SetColumnSpan(richTextBlock, Grid.GetColumnSpan(textBlock));

        return richTextBlock;
    }
}