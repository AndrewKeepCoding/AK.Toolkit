using Microsoft.UI.Xaml;

namespace AK.Toolkit.WinUI3.Localization;

public partial class Localizer
{
    public static readonly DependencyProperty UidProperty = DependencyProperty.RegisterAttached(
        "Uid",
        typeof(string),
        typeof(Localizer),
        new PropertyMetadata(default));

    public static string GetUid(DependencyObject obj) => (string)obj.GetValue(UidProperty);

    public static void SetUid(DependencyObject obj, string value) => obj.SetValue(UidProperty, value);
}