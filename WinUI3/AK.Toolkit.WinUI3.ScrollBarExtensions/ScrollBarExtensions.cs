using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

public partial class ScrollBarExtensions : DependencyObject
{
    public static readonly DependencyProperty KeepVerticalExpandedProperty = DependencyProperty.RegisterAttached(
        "KeepVerticalExpanded",
        typeof(bool),
        typeof(ScrollBarExtensions),
        new PropertyMetadata(false, (d, e) =>
        {
            if (d is FrameworkElement frameworkElement &&
                e.NewValue is bool keepVerticalExpanded &&
                IgnoreAllKeepExpandedPropertyChangedEvents is false)
            {
                OnKeepExpandedPropertyChanged(frameworkElement, Orientation.Vertical, keepVerticalExpanded);
            }
        }));

    public static readonly DependencyProperty KeepHorizontalExpandedProperty = DependencyProperty.RegisterAttached(
        "KeepHorizontalExpanded",
        typeof(bool),
        typeof(ScrollBarExtensions),
        new PropertyMetadata(false, (d, e) =>
        {
            if (d is FrameworkElement frameworkElement &&
                e.NewValue is bool keepHorizontalExpanded &&
                IgnoreAllKeepExpandedPropertyChangedEvents is false)
            {
                OnKeepExpandedPropertyChanged(frameworkElement, Orientation.Horizontal, keepHorizontalExpanded);
            }
        }));

    public static readonly DependencyProperty VerticalAnnotationsProperty = DependencyProperty.RegisterAttached(
         "Annotations",
         typeof(IEnumerable<IAnnotation>),
         typeof(ScrollBarExtensions),
         new PropertyMetadata(Enumerable.Empty<IAnnotation>(), (d, e) =>
         {
             if (d is FrameworkElement frameworkElement &&
                 e.NewValue is IEnumerable<IAnnotation> annotations)
             {
                 OnAnnotationsPropertyChanged(frameworkElement, Orientation.Vertical, annotations);
             }
         }));

    /// <summary>
    /// This property is for debugging purposes.
    /// </summary>
    public static bool IgnoreAllKeepExpandedPropertyChangedEvents { get; set; } = false;

    /// <summary>
    /// This property is for debugging purposes.
    /// </summary>
    public static bool EnableKeepExpandedDebugLogging { get; set; } = false;

    public static bool GetKeepVerticalExpanded(DependencyObject obj)
    {
        return (bool)obj.GetValue(KeepVerticalExpandedProperty);
    }

    public static void SetKeepVerticalExpanded(DependencyObject obj, bool value)
    {
        obj.SetValue(KeepVerticalExpandedProperty, value);
    }

    public static bool GetKeepHorizontalExpanded(DependencyObject obj)
    {
        return (bool)obj.GetValue(KeepHorizontalExpandedProperty);
    }

    public static void SetKeepHorizontalExpanded(DependencyObject obj, bool value)
    {
        obj.SetValue(KeepHorizontalExpandedProperty, value);
    }

    public static IEnumerable<IAnnotation> GetVerticalAnnotations(DependencyObject obj) => (IEnumerable<IAnnotation>)obj.GetValue(VerticalAnnotationsProperty);

    public static void SetVerticalAnnotations(DependencyObject obj, IEnumerable<IAnnotation> value) => obj.SetValue(VerticalAnnotationsProperty, value);

    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(string message)
    {
        if (ScrollBarExtensions.EnableKeepExpandedDebugLogging is true)
        {
            System.Diagnostics.Debug.WriteLine(message, "ScrollBarExtensions");
        }
    }
}