using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AK.Toolkit.WinUI3;

public static class RichTextBlockExtensions
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(string.Empty, OnTextPropertyChanged));

    public static readonly DependencyProperty HighlightingTextProperty =
        DependencyProperty.RegisterAttached(
            "HighlightingText",
            typeof(string),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(string.Empty, OnTextHighlightingPropertyChanged));

    public static readonly DependencyProperty TextHighlightBackgroundProperty =
        DependencyProperty.RegisterAttached(
            "TextHighlightBackground",
            typeof(Brush),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(new SolidColorBrush(Colors.Yellow), OnTextHighlightingPropertyChanged));

    public static readonly DependencyProperty TextHighlightForegroundProperty =
        DependencyProperty.RegisterAttached(
            "TextHighlightForeground",
            typeof(Brush),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(new SolidColorBrush(Colors.Black), OnTextHighlightingPropertyChanged));

    public static readonly DependencyProperty IsTextHighlightingEnabledProperty =
        DependencyProperty.RegisterAttached(
            "IsTextHighlightingEnabled",
            typeof(bool),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(true, OnTextHighlightingPropertyChanged));

    public static readonly DependencyProperty IsTextHighlightingCaseSensitiveProperty =
        DependencyProperty.RegisterAttached(
            "IsTextHighlightingCaseSensitive",
            typeof(bool),
            typeof(RichTextBlockExtensions),
            new PropertyMetadata(false, OnTextHighlightingPropertyChanged));

    public static string GetText(DependencyObject obj)
    {
        return (string)obj.GetValue(TextProperty);
    }

    public static void SetText(DependencyObject obj, string value)
    {
        obj.SetValue(TextProperty, value);
    }

    public static string GetHighlightingText(DependencyObject obj)
    {
        return (string)obj.GetValue(HighlightingTextProperty);
    }

    public static void SetHighlightingText(DependencyObject obj, string value)
    {
        obj.SetValue(HighlightingTextProperty, value);
    }

    public static Brush GetTextHighlightBackground(DependencyObject obj)
    {
        return (Brush)obj.GetValue(TextHighlightBackgroundProperty);
    }

    public static void SetTextHighlightBackground(DependencyObject obj, Brush value)
    {
        obj.SetValue(TextHighlightBackgroundProperty, value);
    }

    public static Brush GetTextHighlightForeground(DependencyObject obj)
    {
        return (Brush)obj.GetValue(TextHighlightForegroundProperty);
    }

    public static void SetTextHighlightForeground(DependencyObject obj, Brush value)
    {
        obj.SetValue(TextHighlightForegroundProperty, value);
    }

    public static bool GetIsTextHighlightingCaseSensitive(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsTextHighlightingCaseSensitiveProperty);
    }

    public static bool GetIsTextHighlightingEnabled(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsTextHighlightingEnabledProperty);
    }

    public static void SetIsTextHighlightingEnabled(DependencyObject obj, bool value)
    {
        obj.SetValue(IsTextHighlightingEnabledProperty, value);
    }

    public static void SetIsTextHighlightingCaseSensitive(DependencyObject obj, bool value)
    {
        obj.SetValue(IsTextHighlightingCaseSensitiveProperty, value);
    }

    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RichTextBlock richTextBlock ||
            e.NewValue is not string text)
        {
            return;
        }

        richTextBlock.Blocks.Clear();

        Paragraph paragraph = new();
        paragraph.Inlines.Add(new Run { Text = text });
        richTextBlock.Blocks.Add(paragraph);

        RefreshTextHighlighting(richTextBlock);
    }

    private static void OnTextHighlightingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RichTextBlock richTextBlock)
        {
            return;
        }

        RefreshTextHighlighting(richTextBlock);
    }

    private static void RefreshTextHighlighting(RichTextBlock richTextBlock)
    {
        richTextBlock.TextHighlighters.Clear();

        var enabled = GetIsTextHighlightingEnabled(richTextBlock);


        if (GetIsTextHighlightingEnabled(richTextBlock) is false ||
            richTextBlock.Blocks.ToText() is not { Length: > 0 } richTextBlockText ||
            GetHighlightingText(richTextBlock) is not { Length: > 0 } hightlightingText)
        {
            return;
        }

        string pattern = Regex.Escape(hightlightingText);
        RegexOptions regexOptions = GetIsTextHighlightingCaseSensitive(richTextBlock) is true
            ? RegexOptions.None
            : RegexOptions.IgnoreCase;
        Regex regex = new(pattern, regexOptions);
        MatchCollection matches = regex.Matches(richTextBlockText);

        foreach (Match match in matches.Cast<Match>())
        {
            TextHighlighter textHighlighter = new();
            TextRange textRange = new(_StartIndex: match.Index, _Length: match.Length);
            textHighlighter.Ranges.Add(textRange);
            textHighlighter.Foreground = GetTextHighlightForeground(richTextBlock);
            textHighlighter.Background = GetTextHighlightBackground(richTextBlock);
            richTextBlock.TextHighlighters.Add(textHighlighter);
        }
    }

    private static string ToText(this BlockCollection blockCollection)
    {
        return string.Join(
            separator: string.Empty,
            values: blockCollection.Select(block => block.ToText()));
    }

    private static string ToText(this Block block)
    {
        return block switch
        {
            Paragraph paragraph => paragraph.ToText(),
            _ => throw new NotImplementedException(block.ToString()),
        };
    }

    private static string ToText(this Paragraph paragraph) => paragraph.Inlines.ToText();

    private static string ToText(this InlineCollection inlineCollection)
    {
        return string.Join(
            separator: string.Empty,
            values: inlineCollection.Select(inline => inline.ToText()));
    }

    private static string ToText(this Inline inline)
    {
        return inline switch
        {
            Run run => run.ToText(),
            Span span => span.ToText(),
            LineBreak => Environment.NewLine,
            _ => throw new NotImplementedException(inline.ToString()),
        };
    }

    private static string ToText(this Run run) => run.Text;

    private static string ToText(this Span span) => span.Inlines.ToText();
}

