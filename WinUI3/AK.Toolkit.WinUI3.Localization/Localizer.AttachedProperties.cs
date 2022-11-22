using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3.Localization;

public partial class Localizer
{
    public static readonly DependencyProperty UidProperty = DependencyProperty.RegisterAttached(
        "Uid",
        typeof(string),
        typeof(Localizer),
        new PropertyMetadata(default));

    private static HashSet<(string Uid, Type DependencyObjectType)> Uids { get; } = new();

    public static IEnumerable<(string Uid, Type DepdencyObjectType)> GetUids() => Uids.ToList();

    public static string GetUid(DependencyObject obj) => (string)obj.GetValue(UidProperty);

    public static void SetUid(DependencyObject obj, string value)
    {
        obj.SetValue(UidProperty, value);
        _ = Uids.Add((Uid: value, DependencyObjectType: obj.GetType()));
    }
}