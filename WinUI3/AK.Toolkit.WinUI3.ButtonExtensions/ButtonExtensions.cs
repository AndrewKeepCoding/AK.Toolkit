using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace AK.Toolkit.WinUI3;

public static class ButtonExtensions
{
    public static readonly DependencyProperty PointerOverBackgroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PointerOverBackgroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(1.0, OnPointerOverBackgroundLightnessFactorPropertyChanged));

    public static readonly DependencyProperty PressedBackgroundLightnessFactorProperty =
        DependencyProperty.RegisterAttached(
            "PressedBackgroundLightnessFactor",
            typeof(double),
            typeof(ButtonExtensions),
            new PropertyMetadata(1.0, OnPressedBackgroundLightnessFactorPropertyChanged));

    public static double GetPointerOverBackgroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PointerOverBackgroundLightnessFactorProperty);

    public static double GetPressedBackgroundLightnessFactor(DependencyObject obj) => (double)obj.GetValue(PressedBackgroundLightnessFactorProperty);

    public static void SetPointerOverBackgroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PointerOverBackgroundLightnessFactorProperty, value);

    public static void SetPressedBackgroundLightnessFactor(DependencyObject obj, double value) => obj.SetValue(PressedBackgroundLightnessFactorProperty, value);

    private static void OnPointerOverBackgroundLightnessFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Button button ||
            e.NewValue is not double backgroundLightnessFactor)
        {
            return;
        }

        _ = button.RegisterPropertyChangedCallback(
            Button.BackgroundProperty, (_, _) =>
            {
                UpdatePointerOverBackgroundLightness(button, backgroundLightnessFactor);
            });

        UpdatePointerOverBackgroundLightness(button, backgroundLightnessFactor);
    }

    private static void OnPressedBackgroundLightnessFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Button button ||
            e.NewValue is not double backgroundLightnessFactor)
        {
            return;
        }

        _ = button.RegisterPropertyChangedCallback(
            Button.BackgroundProperty, (_, _) =>
            {
                UpdatePressedBackgroundLightness(button, backgroundLightnessFactor);
            });

        UpdatePressedBackgroundLightness(button, backgroundLightnessFactor);
    }

    private static void UpdatePointerOverBackgroundLightness(Button button, double backgroundLightnessFactor)
    {
        if (button.Background is not SolidColorBrush backgroundBrush)
        {
            return;
        }

        HslColor hsl = backgroundBrush.Color.ToHsl();
        hsl.L = Math.Max(Math.Min(hsl.L * backgroundLightnessFactor, 1.0), 0.0);
        Color newColor = ColorHelper.FromHsl(
            hue: hsl.H,
            saturation: hsl.S,
            lightness: hsl.L);

        button.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(newColor);
    }

    private static void UpdatePressedBackgroundLightness(Button button, double backgroundLightnessFactor)
    {
        if (button.Background is not SolidColorBrush backgroundBrush)
        {
            return;
        }

        HslColor hsl = backgroundBrush.Color.ToHsl();
        hsl.L = Math.Max(Math.Min(hsl.L * backgroundLightnessFactor, 1.0), 0.0);
        Color newColor = ColorHelper.FromHsl(
            hue: hsl.H,
            saturation: hsl.S,
            lightness: hsl.L);

        button.Resources["ButtonBackgroundPressed"] = new SolidColorBrush(newColor);
    }
}
