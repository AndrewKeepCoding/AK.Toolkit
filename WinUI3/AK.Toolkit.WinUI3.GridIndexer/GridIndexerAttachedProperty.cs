using Microsoft.UI.Xaml;

namespace AK.Toolkit.WinUI3.GridIndexer;

public sealed class GI : DependencyObject
{
    public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached(
        "Row",
        typeof(string),
        typeof(GI),
        new PropertyMetadata(default));

    public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached(
        "Column",
        typeof(string),
        typeof(GI),
        new PropertyMetadata(default));

    public enum IndexTarget
    {
        Row,
        Column,
    }

    public enum ValueType
    {
        Absolute,
        Relative,
    }

    public static string GetRow(DependencyObject obj) => (string)obj.GetValue(RowProperty);

    public static void SetRow(DependencyObject obj, string value) => obj.SetValue(RowProperty, value);

    public static string GetColumn(DependencyObject obj) => (string)obj.GetValue(ColumnProperty);

    public static void SetColumn(DependencyObject obj, string value) => obj.SetValue(ColumnProperty, value);

    public static (ValueType ValueType, int Value)? GetTypeAndValue(DependencyObject target, IndexTarget indexTarget)
    {
        if (target.GetValue(indexTarget is IndexTarget.Row
            ? RowProperty
            : ColumnProperty) is string stringValue &&
            stringValue.Length > 0)
        {
            if ((stringValue.StartsWith("+") is true || stringValue.StartsWith("-") is true) &&
                int.TryParse(stringValue, out int relativeValue) is true)
            {
                return (ValueType.Relative, relativeValue);
            }
            else if (int.TryParse(stringValue, out int absoluteValue) is true)
            {
                return (ValueType.Absolute, absoluteValue);
            }

            throw new GridIndexerException($"Failed to get type and value from '{stringValue}').");
        }

        return null;
    }
}