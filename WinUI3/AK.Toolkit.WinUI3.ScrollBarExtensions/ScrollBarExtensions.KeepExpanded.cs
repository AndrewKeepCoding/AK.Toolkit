using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
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
                e.NewValue is bool keepVerticalExpanded)
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
                e.NewValue is bool keepHorizontalExpanded)
            {
                OnKeepExpandedPropertyChanged(frameworkElement, Orientation.Horizontal, keepHorizontalExpanded);
            }
        }));

    private static List<RemovingVisualState> RemovedVisualStates { get; } = new();

    private record RemovingVisualState(VisualStateGroup VisualStateGroup, VisualState VisualState);

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

    private static void OnKeepExpandedPropertyChanged(FrameworkElement frameworkElement, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        frameworkElement.Loaded -= Target_Loaded;
        frameworkElement.Loaded += Target_Loaded;
        frameworkElement.Unloaded -= Target_Unloaded;
        frameworkElement.Unloaded += Target_Unloaded;
        _ = ApplyKeepExpanded(frameworkElement, targetScrollBarOrientation, keepExpanded);
    }

    private static void Target_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement target)
        {
            if (GetKeepVerticalExpanded(target) is bool keepVerticalExpanded)
            {
                _ = ApplyKeepExpanded(target, Orientation.Vertical, keepVerticalExpanded);
            }

            if (GetKeepHorizontalExpanded(target) is bool keepHorizontalExpanded)
            {
                _ = ApplyKeepExpanded(target, Orientation.Horizontal, keepHorizontalExpanded);
            }
        }
    }

    private static void Target_Unloaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement target)
        {
            _ = ApplyKeepExpanded(target, Orientation.Vertical, keepExpanded: false);
            _ = ApplyKeepExpanded(target, Orientation.Horizontal, keepExpanded: false);
            target.Unloaded -= Target_Unloaded;
        }
    }

    private static bool ApplyKeepExpanded(FrameworkElement frameworkElement, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        return frameworkElement switch
        {
            ScrollBar scrollBar => ApplyKeepExpandedToScrollBar(scrollBar, targetScrollBarOrientation, keepExpanded),
            ScrollViewer scrollViewer => ApplyKeepExpandedToScrollViewer(scrollViewer, targetScrollBarOrientation, keepExpanded),
            NavigationView navigationView => ApplyKeepExpandedToNavigationView(navigationView, targetScrollBarOrientation, keepExpanded),
            _ => ApplyKeepExpandedToUnknownTarget(frameworkElement, targetScrollBarOrientation, keepExpanded),
        };
    }

    private static bool ApplyKeepExpandedToScrollBar(ScrollBar scrollBar, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        if (scrollBar.Orientation != targetScrollBarOrientation)
        {
            return true;
        }

        if (keepExpanded is true)
        {
            _ = VisualStateManager.GoToState(scrollBar, "Expanded", true);
            RemoveVisualStatesFromScrollBar(scrollBar);
        }
        else
        {
            RestoreVisualStates(scrollBar);
            _ = VisualStateManager.GoToState(scrollBar, "Collapsed", true);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToScrollViewer(ScrollViewer scrollViewer, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        string targetScrollBarName = targetScrollBarOrientation is Orientation.Vertical
            ? "VerticalScrollBar"
            : "HorizontalScrollBar";

        if (scrollViewer.FindChildOfName(targetScrollBarName) is ScrollBar scrollBar &&
            ApplyKeepExpandedToScrollBar(scrollBar, targetScrollBarOrientation, keepExpanded) is true)
        {
            if (keepExpanded is true)
            {
                _ = VisualStateManager.GoToState(scrollViewer, "MouseIndicator", true);
                RemoveVisualStatesFromScrollViewer(scrollViewer);
            }
            else
            {
                RestoreVisualStates(scrollViewer);
                _ = VisualStateManager.GoToState(scrollViewer, "NoIndicator", true);
            }
        }

        return true;
    }

    private static bool ApplyKeepExpandedToNavigationView(NavigationView navigationView, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        if (navigationView.FindChildOfName("MenuItemsScrollViewer") is ScrollViewer menuItemsScrollViewer)
        {
            _ = ApplyKeepExpandedToScrollViewer(menuItemsScrollViewer, targetScrollBarOrientation, keepExpanded);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToUnknownTarget(FrameworkElement frameworkElement, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        foreach (ScrollViewer scrollViewer in frameworkElement.FindChildrenOfType<ScrollViewer>())
        {
            _ = ApplyKeepExpandedToScrollViewer(scrollViewer, targetScrollBarOrientation, keepExpanded);
        }

        return true;
    }

    private static void RemoveVisualStatesFromScrollBar(ScrollBar scrollBar)
    {
        if (scrollBar.FindChildOfName("Root") is FrameworkElement root)
        {
            RemoveVisualState(root, "ScrollingIndicatorStates", "TouchIndicator");
            RemoveVisualState(root, "ScrollingIndicatorStates", "NoIndicator");
            RemoveVisualState(root, "ConsciousStates", "CollapsedWithoutAnimation");
            RemoveVisualState(root, "ConsciousStates", "Collapsed");
        }
    }

    private static void RemoveVisualStatesFromScrollViewer(ScrollViewer scrollViewer)
    {
        if (scrollViewer.FindChildOfName("Root") is FrameworkElement root)
        {
            RemoveVisualState(root, "ScrollingIndicatorStates", "TouchIndicator");
            RemoveVisualState(root, "ScrollingIndicatorStates", "NoIndicator");
        }
    }

    private static void RemoveVisualState(FrameworkElement root, string visualStateGroupName, string visualStateName)
    {
        if (VisualStateManager.GetVisualStateGroups(root)
                .FirstOrDefault(x => x.Name == visualStateGroupName) is VisualStateGroup visualStateGroup &&
            visualStateGroup.States
                .FirstOrDefault(x => x.Name == visualStateName) is VisualState removingVisualState)
        {
            RemovedVisualStates.Add(new RemovingVisualState(visualStateGroup, removingVisualState));
            _ = visualStateGroup.States.Remove(removingVisualState);
        }
    }

    private static void RestoreVisualStates(FrameworkElement frameworkElement)
    {
        if (frameworkElement.FindChildOfName("Root") is FrameworkElement root)
        {
            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                IEnumerable<RemovingVisualState> restoring = RemovedVisualStates
                    .Where(x => x.VisualStateGroup == visualStateGroup);

                foreach (VisualState visualState in restoring.Select(x => x.VisualState))
                {
                    visualStateGroup.States.Add(visualState);
                }

                foreach (RemovingVisualState item in restoring.ToList())
                {
                    _ = RemovedVisualStates.Remove(item);
                }
            }
        }
    }
}