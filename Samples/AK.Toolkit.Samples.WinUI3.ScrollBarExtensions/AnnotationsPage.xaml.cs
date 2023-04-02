using AK.Toolkit.WinUI3;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
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

[ObservableObject]
public sealed partial class AnnotationsPage : Page
{
    [ObservableProperty]
    public ObservableCollection<IAnnotation> annotations = new();

    [ObservableProperty]
    private string redAnnotationsText = string.Empty;

    [ObservableProperty]
    private string greenAnnotationsText = string.Empty;

    [ObservableProperty]
    private string blueAnnotationsText = string.Empty;

    [ObservableProperty]
    private double annotationsHeight = 1.0;

    public AnnotationsPage()
    {
        InitializeComponent();

        AnnotationsWidth = (double)Resources["ScrollBarSize"] / 3;
        AnnotationsHeight = 2.0;
        RedAnnotationsLeftOffset = 0.0;
        GreenAnnotationsLeftOffset = RedAnnotationsLeftOffset + AnnotationsWidth;
        BlueAnnotationsLeftOffset = GreenAnnotationsLeftOffset + AnnotationsWidth;

        Users = new ObservableCollection<User>(
            new Faker<User>()
                .UseSeed(0)
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(user => user.FirstName, faker => faker.Name.FirstName())
                .RuleFor(user => user.LastName, faker => faker.Name.LastName())
                .RuleFor(user => user.Address, faker => faker.Address.State())
                .Generate(100));
    }

    public ObservableCollection<User> Users { get; } = new();

    private double AnnotationsWidth { get; set; }

    private double RedAnnotationsLeftOffset { get; set; }

    private double GreenAnnotationsLeftOffset { get; set; }

    private double BlueAnnotationsLeftOffset { get; set; }

    partial void OnRedAnnotationsTextChanged(string value)
    {
        foreach (ColorAnnotation removingAnnotation in Annotations
            .OfType<ColorAnnotation>()
            .Where(x => x.Color == Colors.HotPink)
            .ToList())
        {
            _ = Annotations.Remove(removingAnnotation);
        }

        foreach (User user in GetFilteredUsers(RedAnnotationsText))
        {
            Annotations.Add(
                CreateAnnotation(
                    value: ((double)user.Id / Users.Count) * 100,
                    width: AnnotationsWidth,
                    height: AnnotationsHeight,
                    leftOffset: RedAnnotationsLeftOffset,
                    color: Colors.HotPink));
        }
    }

    partial void OnGreenAnnotationsTextChanged(string value)
    {
        foreach (ColorAnnotation removingAnnotation in Annotations
            .OfType<ColorAnnotation>()
            .Where(x => x.Color == Colors.LightGreen)
            .ToList())
        {
            _ = Annotations.Remove(removingAnnotation);
        }

        foreach (User user in GetFilteredUsers(GreenAnnotationsText))
        {
            Annotations.Add(
                CreateAnnotation(
                    value: ((double)user.Id / Users.Count) * 100,
                    width: AnnotationsWidth,
                    height: AnnotationsHeight,
                    leftOffset: GreenAnnotationsLeftOffset,
                    color: Colors.LightGreen));
        }
    }

    partial void OnBlueAnnotationsTextChanged(string value)
    {
        foreach (ColorAnnotation removingAnnotation in Annotations
            .OfType<ColorAnnotation>()
            .Where(x => x.Color == Colors.SkyBlue)
            .ToList())
        {
            _ = Annotations.Remove(removingAnnotation);
        }

        foreach (User user in GetFilteredUsers(BlueAnnotationsText))
        {
            Annotations.Add(
                CreateAnnotation(
                    value: ((double)user.Id / Users.Count) * 100,
                    width: AnnotationsWidth,
                    height: AnnotationsHeight,
                    leftOffset: BlueAnnotationsLeftOffset,
                    color: Colors.SkyBlue));
        }
    }

    partial void OnAnnotationsHeightChanged(double value)
    {
        OnRedAnnotationsTextChanged(RedAnnotationsText);
        OnGreenAnnotationsTextChanged(GreenAnnotationsText);
        OnBlueAnnotationsTextChanged(BlueAnnotationsText);
    }

    private IAnnotation CreateAnnotation(double value, double width = 16, double height = 1, double leftOffset = 0.0, Color color = default, double opacity = 0.9)
    {
        return new ColorAnnotation(
            value,
            shape: new Rectangle
            {
                Fill = new SolidColorBrush(color),
                Height = height,
                Width = AnnotationsWidth,
                Opacity = opacity,
            },
            color)
        {
            LeftOffset = leftOffset
        };
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
                : Enumerable.Empty<User>();
    }
}