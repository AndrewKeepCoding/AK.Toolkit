using AK.Toolkit.WinUI3;
using Bogus;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;

namespace AK.Toolkit.Samples.WinUI3.ScrollBarExtensions;

public record User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}

public record ColorAnnotation : BasicAnnotation
{
    public ColorAnnotation(double value, Shape shape, Color color) : base(value, shape)
    {
        Color = color;
    }

    public Color Color { get; }
}

public class AnnotationSettings(string name, Color color, double leftOffset, double width)
{
    public string Name { get; } = name;

    public Color Color { get; } = color;

    public double LeftOffset { get; } = leftOffset;

    public double Width { get; } = width;
}

public sealed partial class AnnotationsPage : Page
{
    public AnnotationsPage()
    {
        InitializeComponent();

        double annotationWidth = (double)Resources["ScrollBarSize"] / 3;
        double leftOffset = 0.0;
        AnnotationSettingsList.Add(new AnnotationSettings("Red", Colors.HotPink, leftOffset, annotationWidth));
        leftOffset += annotationWidth;
        AnnotationSettingsList.Add(new AnnotationSettings("Green", Colors.LightGreen, leftOffset, annotationWidth));
        leftOffset += annotationWidth;
        AnnotationSettingsList.Add(new AnnotationSettings("Blue", Colors.SkyBlue, leftOffset, annotationWidth));

        this.AnnotationHeightSlider.Value = 2.0;
        this.AnnotationOpacitySlider.Value = 0.8;

        Users = new ObservableCollection<User>(
            new Faker<User>()
                .UseSeed(0)
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(user => user.FirstName, faker => faker.Name.FirstName())
                .RuleFor(user => user.LastName, faker => faker.Name.LastName())
                .RuleFor(user => user.Address, faker => faker.Address.State())
                .Generate(100));
    }

    private ObservableCollection<IAnnotation> Annotations { get; } = [];

    private ObservableCollection<User> Users { get; }

    private List<AnnotationSettings> AnnotationSettingsList { get; } = [];

    public static SolidColorBrush ToBrush(Color color) => new(color);

    public static string ToString(double value, int decimalPlaces) => value.ToString($"F{decimalPlaces}");

    private static ColorAnnotation CreateAnnotation(double value, double width, double height, double leftOffset, Color color, double opacity)
        => new(
            value,
            shape: new Rectangle
            {
                Fill = new SolidColorBrush(color),
                Height = height,
                Width = width,
                Opacity = opacity,
            },
            color)
        {
            LeftOffset = leftOffset
        };

    private void RefreshAnnotations(string filterText, Color color, double leftOffset, double width, double height, double opacity)
    {
        foreach (ColorAnnotation removingAnnotation in Annotations
            .OfType<ColorAnnotation>()
            .Where(x => x.Color == color)
            .ToList())
        {
            _ = Annotations.Remove(removingAnnotation);
        }

        foreach (User user in GetFilteredUsers(filterText))
        {
            Annotations.Add(
                CreateAnnotation(
                    value: ((double)user.Id / Users.Count) * 100,
                    width,
                    height,
                    leftOffset,
                    color,
                    opacity));
        }
    }

    private IEnumerable<User> GetFilteredUsers(string filter)
    {
        return
            string.IsNullOrEmpty(filter) is false
                ? Users.Where(x =>
                    x.Id.ToString().Contains(filter) ||
                    x.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    x.Address.Contains(filter, StringComparison.OrdinalIgnoreCase))
                : [];
    }

    private void AnnotationSettingsTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox ||
            textBox.DataContext is not AnnotationSettings settings)
        {
            return;
        }

        RefreshAnnotations(
            textBox.Text,
            settings.Color,
            settings.LeftOffset,
            settings.Width,
            this.AnnotationHeightSlider.Value,
            this.AnnotationOpacitySlider.Value);
    }

    private void AnnotationHeightSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        RefreshAnnotationsHeight(e.NewValue);
    }

    private void AnnotationOpacitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        RefreshAnnotationsOpacity(e.NewValue);
    }

    private void RefreshAnnotationsHeight(double height)
    {
        if (double.IsNaN(height) || height <= 0.0)
        {
            return;
        }

        foreach (ColorAnnotation annotation in Annotations.OfType<ColorAnnotation>().ToList())
        {
            _ = Annotations.Remove(annotation);
            Annotations.Add(
                CreateAnnotation(
                    annotation.Value,
                    annotation.Shape.Width,
                    height,
                    annotation.LeftOffset,
                    annotation.Color,
                    this.AnnotationOpacitySlider.Value));
        }
    }

    private void RefreshAnnotationsOpacity(double opacity)
    {
        if (double.IsNaN(opacity) || opacity < 0.0 || opacity > 1.0)
        {
            return;
        }

        foreach (ColorAnnotation annotation in Annotations.OfType<ColorAnnotation>().ToList())
        {
            _ = Annotations.Remove(annotation);
            Annotations.Add(
                CreateAnnotation(
                    annotation.Value,
                    annotation.Shape.Width,
                    annotation.Shape.Height,
                    annotation.LeftOffset,
                    annotation.Color,
                    opacity));
        }
    }
}