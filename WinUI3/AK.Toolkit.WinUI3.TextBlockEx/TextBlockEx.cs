using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System.Linq;
using System.Text.RegularExpressions;

namespace AK.Toolkit.WinUI3;

public sealed class TextBlockEx : Control
{
    public static readonly DependencyProperty IsHighlighterEnabledProperty =
        DependencyProperty.Register(
            nameof(IsHighlighterEnabled),
            typeof(bool),
            typeof(TextBlockEx),
            new PropertyMetadata(true, (d, e) => (d as TextBlockEx)?.RefreshHighlightingText()));

    public static readonly DependencyProperty IsHighlighterCaseSensitiveProperty =
        DependencyProperty.Register(
            nameof(IsHighlighterCaseSensitive),
            typeof(bool),
            typeof(TextBlockEx),
            new PropertyMetadata(default, (d, e) => (d as TextBlockEx)?.RefreshHighlightingText()));

    public static readonly DependencyProperty HighlighterBackgroundProperty =
        DependencyProperty.Register(
            nameof(HighlighterBackground),
            typeof(Brush),
            typeof(TextBlockEx),
            new PropertyMetadata(default));

    public static readonly DependencyProperty HighlighterForegroundProperty =
        DependencyProperty.Register(
            nameof(HighlighterForeground),
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
        HighlighterForeground = Foreground;
    }

    public bool IsHighlighterEnabled
    {
        get => (bool)GetValue(IsHighlighterEnabledProperty);
        set => SetValue(IsHighlighterEnabledProperty, value);
    }

    public bool IsHighlighterCaseSensitive
    {
        get => (bool)GetValue(IsHighlighterCaseSensitiveProperty);
        set => SetValue(IsHighlighterCaseSensitiveProperty, value);
    }

    public string HighlightingText
    {
        get => (string)GetValue(HighlightingTextProperty);
        set => SetValue(HighlightingTextProperty, value);
    }

    public Brush HighlighterBackground
    {
        get => (Brush)GetValue(HighlighterBackgroundProperty);
        set => SetValue(HighlighterBackgroundProperty, value);
    }

    public Brush HighlighterForeground
    {
        get => (Brush)GetValue(HighlighterForegroundProperty);
        set => SetValue(HighlighterForegroundProperty, value);
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

        if (IsHighlighterEnabled is false ||
            string.IsNullOrEmpty(HighlightingText))
        {
            return;
        }

        string pattern = Regex.Escape(HighlightingText);
        RegexOptions regexOptions = IsHighlighterCaseSensitive is true
            ? RegexOptions.None
            : RegexOptions.IgnoreCase;
        Regex regex = new(pattern, regexOptions);
        MatchCollection matches = regex.Matches(Text);

        foreach (Match match in matches.Cast<Match>())
        {
            TextHighlighter textHighlighter = new();
            TextRange textRange = new(_StartIndex: match.Index, _Length: match.Length);
            textHighlighter.Ranges.Add(textRange);
            textHighlighter.Foreground = HighlighterForeground;
            textHighlighter.Background = HighlighterBackground;
            RichTextBlockControl.TextHighlighters.Add(textHighlighter);
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
