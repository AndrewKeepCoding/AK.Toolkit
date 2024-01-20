using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AK.Toolkit.WinUI3;

public sealed class TextBlockEx : Control
{
    public static readonly DependencyProperty IsTextHighlightEnabledProperty =
        DependencyProperty.Register(
            nameof(IsTextHighlightEnabled),
            typeof(bool),
            typeof(TextBlockEx),
            new PropertyMetadata(true, (d, e) => (d as TextBlockEx)?.RefreshHighlightingText()));

    public static readonly DependencyProperty IsTextHighlightCaseSensitiveProperty =
        DependencyProperty.Register(
            nameof(IsTextHighlightCaseSensitive),
            typeof(bool),
            typeof(TextBlockEx),
            new PropertyMetadata(default, (d, e) => (d as TextBlockEx)?.RefreshHighlightingText()));

    public static readonly DependencyProperty TextHighlightBackgroundProperty =
        DependencyProperty.Register(
            nameof(TextHighlightBackground),
            typeof(Brush),
            typeof(TextBlockEx),
            new PropertyMetadata(default));

    public static readonly DependencyProperty TextHighlightForegroundProperty =
        DependencyProperty.Register(
            nameof(TextHighlightForeground),
            typeof(Brush),
            typeof(TextBlockEx),
            new PropertyMetadata(default));

    public static readonly DependencyProperty HighlightingTextProperty =
        DependencyProperty.Register(
            nameof(HighlightingText),
            typeof(string),
            typeof(TextBlockEx),
            new PropertyMetadata(default, (d, e) => (d as TextBlockEx)?.RefreshHighlightingText()));

    public static readonly DependencyProperty TextAlignmentProperty =
        DependencyProperty.Register(
            nameof(TextAlignment),
            typeof(TextAlignment),
            typeof(TextBlockEx),
            new PropertyMetadata(TextAlignment.Left));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(TextBlockEx),
            new PropertyMetadata(default, (d, e) => (d as TextBlockEx)?.RefreshText()));

    public static readonly DependencyProperty TextWrappingProperty =
        DependencyProperty.Register(
            nameof(TextWrapping),
            typeof(TextWrapping),
            typeof(TextBlockEx),
            new PropertyMetadata(default));

    public TextBlockEx()
    {
        DefaultStyleKey = typeof(TextBlockEx);
    }

    public event EventHandler? TextHighlighted;

    public event EventHandler? TextUnhighlighted;

    public bool IsTextHighlightEnabled
    {
        get => (bool)GetValue(IsTextHighlightEnabledProperty);
        set => SetValue(IsTextHighlightEnabledProperty, value);
    }

    public bool IsTextHighlightCaseSensitive
    {
        get => (bool)GetValue(IsTextHighlightCaseSensitiveProperty);
        set => SetValue(IsTextHighlightCaseSensitiveProperty, value);
    }

    public string HighlightingText
    {
        get => (string)GetValue(HighlightingTextProperty);
        set => SetValue(HighlightingTextProperty, value);
    }

    public Brush TextHighlightBackground
    {
        get => (Brush)GetValue(TextHighlightBackgroundProperty);
        set => SetValue(TextHighlightBackgroundProperty, value);
    }

    public Brush TextHighlightForeground
    {
        get => (Brush)GetValue(TextHighlightForegroundProperty);
        set => SetValue(TextHighlightForegroundProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    public TextWrapping TextWrapping
    {
        get => (TextWrapping)GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    private RichTextBlock? RichTextBlockControl { get; set; }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        RichTextBlockControl = GetTemplateChild(nameof(RichTextBlockControl)) as RichTextBlock;
        RefreshText();
    }

    private void RefreshHighlightingText()
    {
        if (RichTextBlockControl is null)
        {
            return;
        }

        RichTextBlockControl.TextHighlighters.Clear();

        if (IsTextHighlightEnabled is false ||
            string.IsNullOrEmpty(Text) ||
            string.IsNullOrEmpty(HighlightingText))
        {
            return;
        }

        string pattern = Regex.Escape(HighlightingText);
        RegexOptions regexOptions = IsTextHighlightCaseSensitive is true
            ? RegexOptions.None
            : RegexOptions.IgnoreCase;
        Regex regex = new(pattern, regexOptions);
        MatchCollection matches = regex.Matches(Text);

        foreach (Match match in matches.Cast<Match>())
        {
            TextHighlighter textHighlighter = new();
            TextRange textRange = new(_StartIndex: match.Index, _Length: match.Length);
            textHighlighter.Ranges.Add(textRange);
            textHighlighter.Foreground = TextHighlightForeground;
            textHighlighter.Background = TextHighlightBackground;
            RichTextBlockControl.TextHighlighters.Add(textHighlighter);
        }

        if (matches.Count > 0)
        {
            TextHighlighted?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            TextUnhighlighted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void RefreshText()
    {
        if (RichTextBlockControl is null)
        {
            return;
        }

        RichTextBlockControl.Blocks.Clear();

        Paragraph paragraph = new()
        {
            Inlines = { new Run { Text = Text, }, },
        };

        RichTextBlockControl.Blocks.Add(paragraph);

        RefreshHighlightingText();
    }
}
