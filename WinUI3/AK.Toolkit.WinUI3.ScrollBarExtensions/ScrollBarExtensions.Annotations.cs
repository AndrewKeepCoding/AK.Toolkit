using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;

namespace AK.Toolkit.WinUI3;

public enum ValueType
{
    Relative,
    Absolute,
}

public interface IAnnotation
{
    double Value { get; }

    ValueType ValueType { get; }

    Shape Shape { get; }

    double LeftOffset { get; }
}

public record BasicAnnotation : IAnnotation
{
    public double Value { get; }

    public ValueType ValueType { get; set; }

    public Shape Shape { get; private set; }

    public double LeftOffset { get; set; }

    public BasicAnnotation(double value, Shape shape)
    {
        Value = value;
        Shape = shape;
    }
}

public partial class ScrollBarExtensions
{
    private static Dictionary<ScrollBar, AnnotationsPresenter> AnnotationPresenters { get; } = [];

    private static void OnAnnotationsPropertyChanged(FrameworkElement annotationsTarget, Orientation orientation, IEnumerable<IAnnotation> annotations)
    {
        Log($"OnAnnotationsPropertyChanged FrameworkElement: {annotationsTarget} / Orientation: {orientation} / Annotations: {annotations.Count()}");

        annotationsTarget.Loaded -= AnnotationsTarget_Loaded;
        annotationsTarget.Loaded += AnnotationsTarget_Loaded;
        annotationsTarget.Unloaded -= AnnotationsTarget_Unloaded;
        annotationsTarget.Unloaded += AnnotationsTarget_Unloaded;

        ApplyAnnotations(annotationsTarget, orientation, annotations);
    }

    private static void AnnotationsTarget_Loaded(object sender, RoutedEventArgs e)
    {
        Log($"AnnotationsTarget_Loaded sender: {sender}");

        if (sender is FrameworkElement target)
        {
            target.Loaded -= AnnotationsTarget_Loaded;
            ApplyAnnotations(target);
        }
    }

    private static void AnnotationsTarget_Unloaded(object sender, RoutedEventArgs e)
    {
        Log($"AnnotationsTarget_Unloaded sender: {sender}");

        if (sender is FrameworkElement target)
        {
            target.Unloaded -= AnnotationsTarget_Unloaded;
            ApplyAnnotations(target, Orientation.Vertical, []);
        }
    }

    private static void ApplyAnnotations(FrameworkElement target)
    {
        if (GetVerticalAnnotations(target) is IEnumerable<IAnnotation> verticalAnnotations)
        {
            ApplyAnnotations(target, Orientation.Vertical, verticalAnnotations);
        }
    }

    private static void ApplyAnnotations(FrameworkElement frameworkElement, Orientation targetScrollBarOrientation, IEnumerable<IAnnotation> annotations)
    {
        _ = frameworkElement switch
        {
            ScrollBar scrollBar => ApplyAnnotationsToScrollBar(scrollBar, targetScrollBarOrientation, annotations),
            ScrollViewer scrollViewer => ApplyAnnotationsToScrollViewer(scrollViewer, targetScrollBarOrientation, annotations),
            ListView listView => ApplyAnnotationsToListView(listView, targetScrollBarOrientation, annotations),
            _ => ApplyAnnotationsToUnknownTarget(frameworkElement, targetScrollBarOrientation, annotations),
        };
    }

    private static bool ApplyAnnotationsToScrollBar(ScrollBar scrollBar, Orientation orientation, IEnumerable<IAnnotation> annotations)
    {
        Log($"ApplyAnnotationsToScrollBar {scrollBar.Orientation}ScrollBar ({scrollBar.GetHashCode()}) Orientation: {orientation} / Annotations: {annotations}");

        if (scrollBar.Orientation == orientation)
        {
            scrollBar.EffectiveViewportChanged -= Annotations_EffectiveViewportChanged;
            scrollBar.EffectiveViewportChanged += Annotations_EffectiveViewportChanged;
        }

        ApplyAnnotationsProperty(scrollBar, annotations);

        return true;
    }

    private static bool ApplyAnnotationsToScrollViewer(ScrollViewer scrollViewer, Orientation orientation, IEnumerable<IAnnotation> annotations)
    {
        Log($"ApplyAnnotationsToScrollViewer ScrollViewer ({scrollViewer.GetHashCode()}) Orientation: {orientation} / Annotations: {annotations.Count()}");

        scrollViewer.EffectiveViewportChanged -= Annotations_EffectiveViewportChanged;
        scrollViewer.EffectiveViewportChanged += Annotations_EffectiveViewportChanged;

        string targetScrollBarName = orientation is Orientation.Vertical
            ? "VerticalScrollBar"
            : "HorizontalScrollBar";

        if (scrollViewer.FindDescendant(targetScrollBarName) is ScrollBar scrollBar &&
            ApplyAnnotationsToScrollBar(scrollBar, orientation, annotations) is true)
        {
            if (orientation is Orientation.Vertical)
            {
                SetVerticalAnnotations(scrollBar, annotations);
            }
        }

        return true;
    }

    private static bool ApplyAnnotationsToListView(ListView listView, Orientation orientation, IEnumerable<IAnnotation> annotations)
    {
        Log($"ApplyAnnotationsToListView ListView ({listView.GetHashCode()}) Orientation: {orientation} / Annotations: {annotations.Count()}");

        if (orientation is Orientation.Vertical &&
            listView.FindDescendant("ScrollViewer") is ScrollViewer scrollViewer)
        {
            scrollViewer.Loaded -= Annotations_ScrollViewer_Loaded;
            scrollViewer.Loaded += Annotations_ScrollViewer_Loaded;
            SetVerticalAnnotations(scrollViewer, annotations);
        }

        return true;
    }

    private static bool ApplyAnnotationsToUnknownTarget(FrameworkElement frameworkElement, Orientation orientation, IEnumerable<IAnnotation> annotations)
    {
        Log($"ApplyAnnotationsToUnknownTarget FrameworkElement ({frameworkElement.GetHashCode()}) Orientation: {orientation} / Annotations: {annotations.Count()}");

        foreach (ScrollBar scrollBar in frameworkElement
            .FindDescendants()
            .OfType<ScrollBar>()
            .Where(x => x.Orientation == orientation))
        {
            if (orientation is Orientation.Vertical)
            {
                SetVerticalAnnotations(scrollBar, annotations);
            }
        }

        foreach (ScrollViewer scrollViewer in frameworkElement.FindDescendants().OfType<ScrollViewer>())
        {
            if (orientation is Orientation.Vertical)
            {
                SetVerticalAnnotations(scrollViewer, annotations);
            }
        }

        return true;
    }

    private static void Annotations_ScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
        Log($"Annotations_ScrollViewer_Loaded ScrollViewer ({sender.GetHashCode()})");

        if (sender is ScrollViewer scrollViewer)
        {
            ApplyAnnotations(scrollViewer);
        }
    }

    private static void Annotations_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
    {
        sender.EffectiveViewportChanged -= Annotations_EffectiveViewportChanged;

        if (sender is ScrollBar scrollBar)
        {
            IEnumerable<IAnnotation> annotations = ScrollBarExtensions.GetVerticalAnnotations(scrollBar);
            ApplyAnnotationsProperty(scrollBar, annotations);
        }

        ApplyAnnotations(sender);
    }

    private static AnnotationsPresenter? GetAnnotationPresenter(ScrollBar scrollBar)
    {
        if (AnnotationPresenters.TryGetValue(scrollBar, out AnnotationsPresenter? presenter) is not true &&
            scrollBar.FindDescendant("Root") is Grid rootGrid &&
            VisualStateManager.GetVisualStateGroups(rootGrid)
            .FirstOrDefault(x => x.Name == "ConsciousStates") is VisualStateGroup targetVisualStateGroup)
        {
            Thickness margin = new(
                left: 0.0,
                top: (scrollBar.FindDescendant("VerticalSmallDecrease") as Control)?.ActualHeight ?? 0.0,
                right: 0.0,
                bottom: (scrollBar.FindDescendant("VerticalSmallIncrease") as Control)?.ActualHeight ?? 0.0
                );

            presenter = new AnnotationsPresenter()
            {
                Margin = margin,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                IsHitTestVisible = false,
            };

            presenter.SetBinding(AnnotationsPresenter.MinimumProperty, new Binding()
            {
                Source = scrollBar,
                Path = new PropertyPath("Minimum"),
            });

            presenter.SetBinding(AnnotationsPresenter.MaximumProperty, new Binding()
            {
                Source = scrollBar,
                Path = new PropertyPath("Maximum"),
            });

            AnnotationPresenters[scrollBar] = presenter;
            rootGrid.Children.Add(presenter);
        }

        return presenter;
    }

    private static void ApplyAnnotationsProperty(ScrollBar scrollBar, IEnumerable<IAnnotation> annotations)
    {
        if (GetAnnotationPresenter(scrollBar) is AnnotationsPresenter presenter)
        {
            presenter.Annotations = annotations;
        }
    }
}

internal partial class AnnotationsPresenter : Canvas
{
    public static readonly DependencyProperty AnnotationsProperty = DependencyProperty.Register(
        nameof(Annotations),
        typeof(IEnumerable<IAnnotation>),
        typeof(AnnotationsPresenter),
        new PropertyMetadata(
            Enumerable.Empty<IAnnotation>(),
            (d, p) => (d as AnnotationsPresenter)?.UpdateAnnotationsCollectionView()));

    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum),
        typeof(double),
        typeof(AnnotationsPresenter),
        new PropertyMetadata(default, OnMinimumPropertyChanged));

    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(double),
        typeof(AnnotationsPresenter),
        new PropertyMetadata(default, OnMaximumPropertyChanged));

    public AnnotationsPresenter()
    {
        Loaded += AnnotationsPresenter_Loaded;
        SizeChanged += AnnotationsPresenter_SizeChanged;
    }

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public IEnumerable<IAnnotation> Annotations
    {
        get => (IEnumerable<IAnnotation>)GetValue(AnnotationsProperty);
        set => SetValue(AnnotationsProperty, value);
    }

    private CollectionViewSource? CollectionViewSource { get; set; }

    private ICollectionView? CollectionView { get; set; }

    public void UpdateAnnotationsCollectionView()
    {
        if (CollectionView is not null)
        {
            CollectionView.VectorChanged -= CollectionView_VectorChanged;
        }

        CollectionViewSource = new CollectionViewSource
        {
            Source = Annotations
        };

        CollectionView = CollectionViewSource.View;
        CollectionView.VectorChanged += CollectionView_VectorChanged;
    }

    private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as AnnotationsPresenter)?.RedrawAnnotations();
    }

    private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as AnnotationsPresenter)?.RedrawAnnotations();
    }

    private void CollectionView_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
    {
        RedrawAnnotations();
    }

    private void AnnotationsPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        RedrawAnnotations();
    }

    private void RedrawAnnotations()
    {
        Children.Clear();
        double valueRange = Maximum - Minimum;

        if (valueRange is 0.0)
        {
            return;
        }

        foreach (IAnnotation annotation in Annotations)
        {
            double value = annotation.ValueType == ValueType.Relative
                ? valueRange * annotation.Value / 100.0
                : annotation.Value;
            value = (value - Minimum) * ActualHeight / valueRange;

            if (value < 0.0)
            {
                continue;
            }

            Canvas.SetTop(annotation.Shape, value - annotation.Shape.Height / 2);
            Canvas.SetLeft(annotation.Shape, annotation.LeftOffset);
            Children.Add(annotation.Shape);
        }
    }

    private void AnnotationsPresenter_Loaded(object sender, RoutedEventArgs e)
    {
        RedrawAnnotations();
    }
}