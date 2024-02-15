using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace AK.Toolkit.WinUI3.RichTextBlockExtensionsSampleApp;

public class TextHighlightablePage : Page
{
    public static readonly DependencyProperty HighlightingTextProperty =
        DependencyProperty.Register(
            nameof(HighlightingText),
            typeof(string),
            typeof(TextHighlightablePage),
            new PropertyMetadata(default));

    public string HighlightingText
    {
        get => (string)GetValue(HighlightingTextProperty);
        set => SetValue(HighlightingTextProperty, value);
    }
}