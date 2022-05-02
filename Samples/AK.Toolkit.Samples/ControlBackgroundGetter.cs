using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;

namespace AK.Toolkit.Samples;

public class ControlBackgroundGetter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is Control control)
        {
            if (control.Tag is "Default")
            {
                return null;
            }

            return control.Background;
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}