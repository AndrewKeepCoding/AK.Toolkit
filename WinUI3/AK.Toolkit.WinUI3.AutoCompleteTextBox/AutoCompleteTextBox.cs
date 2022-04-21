using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.System;

namespace AK.Toolkit.WinUI3;

/// <summary>
/// A TextBox control that shows a suggestion based on input.
/// The suggestion is shown inside the TextBox control by overriding the placeholder feature.
/// Suggestions need to be provided by the SuggestionsSource property.
/// </summary>
/// <remarks>
/// If you need to change the "FontFamily", use a "Monospaced" font. Otherwise the suggestion might show up misaligned.
/// </remarks>
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

    private string OriginalPlaceholderText { get; set; } = string.Empty;

    private TextBlock? PlaceholderControl { get; set; }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        PlaceholderControl = GetTemplateChild(PlaceholderControlName) as TextBlock;

        if (PlaceholderControl is not null)
        {
            OriginalPlaceholderText = PlaceholderControl.Text;

            TextChanged += (s, e) => UpdateSuggestion(acceptSuggestion: false);

            KeyDown += (s, e) =>
            {
                if (e.Key is VirtualKey.Right)
                {
                    UpdateSuggestion(acceptSuggestion: true);
                }
            };

            LostFocus += (s, e) =>
            {
                if (Text.Length > 0)
                {
                    PlaceholderControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    UpdatePlaceholderControl(OriginalPlaceholderText, Visibility.Visible);
                }
            };

            GettingFocus += (s, e) => UpdateSuggestion(acceptSuggestion: false);
        }
    }

    private static string GetSuggestion(string input, bool ignoreCase, IEnumerable<string> suggestionsSource)
    {
        string suggestion = string.Empty;

        if (input.Length > 0 && suggestionsSource is not null)
        {
            string? result = suggestionsSource.FirstOrDefault(x => x.StartsWith(input, ignoreCase, culture: null));

            if (result is not null)
            {
                suggestion = result;
            }
        }

        return suggestion;
    }

    private void UpdatePlaceholderControl(string text, Visibility visibility)
    {
        if (PlaceholderControl is not null)
        {
            PlaceholderControl.Text = text;
            PlaceholderControl.Visibility = visibility;
        }
    }

    private void UpdateSuggestion(bool acceptSuggestion)
    {
        if (Text.Length == 0 || LastAcceptedSuggestion.Equals(Text) is not true)
        {
            bool ignoreCase = (IsSuggestionCaseSensitive is false);
            string suggestion = GetSuggestion(Text, ignoreCase, SuggestionsSource);

            if (suggestion.Length > 0)
            {
                string text = suggestion[Text.Length..].PadLeft(suggestion.Length);
                text += SuggestionSuffix;
                UpdatePlaceholderControl(text, Visibility.Visible);
            }
            else if (Text.Length == 0)
            {
                UpdatePlaceholderControl(OriginalPlaceholderText, Visibility.Visible);
            }
            else
            {
                UpdatePlaceholderControl(OriginalPlaceholderText, Visibility.Collapsed);
            }

            if (acceptSuggestion is true && suggestion.Length > 0)
            {
                UpdatePlaceholderControl(OriginalPlaceholderText, Visibility.Collapsed);
                Text = suggestion;
                LastAcceptedSuggestion = suggestion;
                SelectionStart = Text.Length;
            }
            else
            {
                LastAcceptedSuggestion = string.Empty;
            }
        }
    }
}
