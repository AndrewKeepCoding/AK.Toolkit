using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using Windows.System;

namespace AK.Toolkit.WinUI3;

/// <summary>
/// A TextBox control that shows a suggestion "inside it self".
/// Suggestions need to be provided by the SuggestionsSource property.
/// </summary>
[TemplatePart(Name = PlaceholderControlName, Type = typeof(TextBlock))]
public sealed class AutoCompleteTextBox : TextBox
{
    /// <summary>
    /// Identifies the <see cref="IsSuggestionCaseSensitive"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty IsSuggestionCaseSensitiveProperty =
        DependencyProperty.Register(
            nameof(IsSuggestionCaseSensitive),
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new PropertyMetadata(false));

    /// <summary>
    /// Identifies the <see cref="SuggestionForeground"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SuggestionForegroundProperty =
        DependencyProperty.Register(
            nameof(SuggestionForeground),
            typeof(Brush),
            typeof(AutoCompleteTextBox),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SuggestionsSource"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SuggestionsSourceProperty =
        DependencyProperty.Register(
            nameof(SuggestionsSource),
            typeof(IEnumerable<string>),
            typeof(AutoCompleteTextBox),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="SuggestionSuffix"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SuggestionSuffixProperty =
        DependencyProperty.Register(
            nameof(SuggestionSuffix),
            typeof(string),
            typeof(AutoCompleteTextBox),
            new PropertyMetadata(string.Empty));

    private const string PlaceholderControlName = "PlaceholderTextContentPresenter";

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoCompleteTextBox"/> class.
    /// </summary>
    public AutoCompleteTextBox()
    {
        this.DefaultStyleKey = typeof(AutoCompleteTextBox);
    }

    /// <summary>
    /// Get or sets a value indicating whether the suggestion is case sensitive.
    /// </summary>
    public bool IsSuggestionCaseSensitive
    {
        get => (bool)GetValue(IsSuggestionCaseSensitiveProperty);
        set => SetValue(IsSuggestionCaseSensitiveProperty, value);
    }

    /// <summary>
    /// Gets or sets a brush that describes the suggestion foreground color.
    /// </summary>
    public Brush SuggestionForeground
    {
        get => (Brush)GetValue(SuggestionForegroundProperty);
        set => SetValue(SuggestionForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets a collection of strings as a source of suggestions.
    /// </summary>
    public IEnumerable<string> SuggestionsSource
    {
        get => (IEnumerable<string>)GetValue(SuggestionsSourceProperty);
        set => SetValue(SuggestionsSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets a suffix string for the suggestion.
    /// </summary>
    public string SuggestionSuffix
    {
        get => (string)GetValue(SuggestionSuffixProperty);
        set => SetValue(SuggestionSuffixProperty, value);
    }

    private string LastAcceptedSuggestion { get; set; } = string.Empty;

    private TextBox SuggestionTextBox { get; } = new TextBox();

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(PlaceholderControlName) is TextBlock placeHolder)
        {
            SuggestionTextBox.FontFamily = FontFamily;
            SuggestionTextBox.FontSize = FontSize;
            SuggestionTextBox.FontStyle = FontStyle;
            SuggestionTextBox.FontWeight = FontWeight;
            SuggestionTextBox.FontStretch = FontStretch;

            SuggestionTextBox.Foreground = SuggestionForeground;
            SuggestionTextBox.IsHitTestVisible = false;
            SuggestionTextBox.Text = string.Empty;

            Grid.SetColumn(SuggestionTextBox, Grid.GetColumn(placeHolder));
            Grid.SetColumnSpan(SuggestionTextBox, Grid.GetColumnSpan(placeHolder));
            Grid.SetRow(SuggestionTextBox, Grid.GetRow(placeHolder));
            Grid.SetRowSpan(SuggestionTextBox, Grid.GetRowSpan(placeHolder));

            if (VisualTreeHelper.GetParent(placeHolder) is Grid parentGrid)
            {
                parentGrid.Children.Insert(0, SuggestionTextBox);
            }

            TextChanged += (s, e) => UpdateSuggestion();

            LostFocus += (s, e) => HideSuggestionControl();

            GotFocus += (s, e) => UpdateSuggestion();

            KeyDown += (s, e) =>
            {
                if (e.Key is VirtualKey.Right)
                {
                    AcceptSuggestion();
                }
            };

            UpdateSuggestion();
        }
    }

    private void HideSuggestionControl() => SuggestionTextBox.Visibility = Visibility.Collapsed;

    private void ShowSuggestionControl() => SuggestionTextBox.Visibility = Visibility.Visible;

    private static string GetSuggestion(string input, bool ignoreCase, IEnumerable<string> suggestionsSource)
    {
        string suggestion = string.Empty;

        if (input.Length > 0 && suggestionsSource is not null)
        {
            string? result = suggestionsSource.FirstOrDefault(x => x.StartsWith(input, ignoreCase, culture: null));

            if (result is not null && result.Equals(input) is not true)
            {
                suggestion = result;
            }
        }

        return suggestion;
    }

    private void AcceptSuggestion()
    {
        ClearSuggestion();

        bool ignoreCase = (IsSuggestionCaseSensitive is false);
        string suggestion = GetSuggestion(Text, ignoreCase, SuggestionsSource);
        if (suggestion.Length > 0)
        {
            Text = suggestion;
            LastAcceptedSuggestion = Text;
            SelectionStart = Text.Length;
        }
    }

    private void ClearSuggestion()
    {
        SuggestionTextBox.Text = string.Empty;
        LastAcceptedSuggestion = string.Empty;
    }

    private void UpdateSuggestion()
    {
        ShowSuggestionControl();

        string suggestion = string.Empty;

        if (LastAcceptedSuggestion.Equals(Text) is not true)
        {
            bool ignoreCase = (IsSuggestionCaseSensitive is false);
            suggestion = GetSuggestion(Text, ignoreCase, SuggestionsSource);

            if (suggestion.Length > 0)
            {
                SuggestionTextBox.Text = $"{Text}{suggestion[Text.Length..]}{SuggestionSuffix}";
            }
        }

        if (suggestion.Length == 0)
        {
            ClearSuggestion();
        }
    }
}