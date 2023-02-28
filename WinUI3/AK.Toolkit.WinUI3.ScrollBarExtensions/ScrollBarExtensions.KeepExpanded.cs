using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

file static class DebugLog
{
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(string message)
    {
        if (ScrollBarExtensions.EnableKeepExpandedDebugLogging is true)
        {
            System.Diagnostics.Debug.WriteLine(message, "ScrollBarExtensions.KeepExpanded");
        }
    }
}

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

    /// <summary>
    /// This property is for debugging purposes.
    /// </summary>
    public static bool IgnoreAllKeepExpandedPropertyChangedEvents { get; set; } = false;

    /// <summary>
    /// This property is for debugging purposes.
    /// </summary>
    public static bool EnableKeepExpandedDebugLogging { get; set; } = false;

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

    private static void OnKeepExpandedPropertyChanged(FrameworkElement target, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"OnKeepExpandedPropertyChanged FrameworkElement: {target} / Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        target.Loaded -= Target_Loaded;
        target.Loaded += Target_Loaded;
        target.Unloaded -= Target_Unloaded;
        target.Unloaded += Target_Unloaded;
        ApplyKeepExpanded(target, orientation, keepExpanded);
    }

    private static void Target_Loaded(object sender, RoutedEventArgs e)
    {
        DebugLog.Log($"Target_Loaded sender: {sender}");

        if (sender is FrameworkElement target)
        {
            target.Loaded -= Target_Loaded;
            ApplyKeepExpanded(target);
        }
    }

    private static void Target_Unloaded(object sender, RoutedEventArgs e)
    {
        DebugLog.Log($"Target_Unloaded sender: {sender}");

        if (sender is FrameworkElement target)
        {
            target.Unloaded -= Target_Unloaded;
            ApplyKeepExpanded(target, Orientation.Vertical, keepExpanded: false);
            ApplyKeepExpanded(target, Orientation.Horizontal, keepExpanded: false);
        }
    }

    private static void ApplyKeepExpanded(FrameworkElement target)
    {
        if (GetKeepVerticalExpanded(target) is bool keepVerticalExpanded)
        {
            ApplyKeepExpanded(target, Orientation.Vertical, keepVerticalExpanded);
        }

        if (GetKeepHorizontalExpanded(target) is bool keepHorizontalExpanded)
        {
            ApplyKeepExpanded(target, Orientation.Horizontal, keepHorizontalExpanded);
        }
    }

    private static void ApplyKeepExpanded(FrameworkElement frameworkElement, Orientation targetScrollBarOrientation, bool keepExpanded)
    {
        _ = frameworkElement switch
        {
            ScrollBar scrollBar => ApplyKeepExpandedToScrollBar(scrollBar, targetScrollBarOrientation, keepExpanded),
            ScrollViewer scrollViewer => ApplyKeepExpandedToScrollViewer(scrollViewer, targetScrollBarOrientation, keepExpanded),
            ListView listView => ApplyKeepExpandedToListView(listView, targetScrollBarOrientation, keepExpanded),
            NavigationView navigationView => ApplyKeepExpandedToNavigationView(navigationView, targetScrollBarOrientation, keepExpanded),
            _ => ApplyKeepExpandedToUnknownTarget(frameworkElement, targetScrollBarOrientation, keepExpanded),
        };
    }

    private static bool ApplyKeepExpandedToScrollBar(ScrollBar scrollBar, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"ApplyKeepExpandedToScrollBar ScrollBar ({scrollBar.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        if (scrollBar.Orientation == orientation)
        {
            UpdateScrollBarVisualStates(scrollBar, keepExpanded);
        }

        return true;
    }

    private static void UpdateScrollBarVisualStates(ScrollBar scrollBar, bool keepExpanded)
    {
        if (keepExpanded is true)
        {
            ChangeVisualState(scrollBar, "Expanded");
            RemoveVisualStatesFromScrollBar(scrollBar);
        }
        else
        {
            RestoreVisualStates(scrollBar);
            ChangeVisualState(scrollBar, "Collapsed");
        }
    }

    private static bool ApplyKeepExpandedToScrollViewer(ScrollViewer scrollViewer, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"ApplyKeepExpandedToScrollViewer ScrollViewer ({scrollViewer.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        scrollViewer.EffectiveViewportChanged -= ScrollViewer_EffectiveViewportChanged;
        scrollViewer.EffectiveViewportChanged += ScrollViewer_EffectiveViewportChanged;

        if (keepExpanded is true)
        {
            if (orientation is Orientation.Vertical)
            {
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
        }

        UpdateScrollViewerVisualStates(scrollViewer, keepExpanded);

        string targetScrollBarName = orientation is Orientation.Vertical
            ? "VerticalScrollBar"
            : "HorizontalScrollBar";

        if (scrollViewer.FindDescendant(targetScrollBarName) is ScrollBar scrollBar &&
            ApplyKeepExpandedToScrollBar(scrollBar, orientation, keepExpanded) is true)
        {
            if (orientation is Orientation.Vertical)
            {
                SetKeepVerticalExpanded(scrollBar, keepExpanded);
            }
            else
            {
                SetKeepHorizontalExpanded(scrollBar, keepExpanded);
            }
        }

        return true;
    }

    private static void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
    {
        DebugLog.Log($"VisualStateGroup_CurrentStateChanged Control: {e.Control.GetType().Name} VisualState: {e.OldState.Name} -> {e.NewState.Name}");
    }

    private static bool ApplyKeepExpandedToListView(ListView listView, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"ApplyKeepExpandedToListView ListView ({listView.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        listView.Tag = keepExpanded is true ? "KeepExpandedIsTrue" : "KeepExpandedIsFalse";

        if (orientation is Orientation.Vertical &&
            listView.FindDescendant("ScrollViewer") is ScrollViewer scrollViewer)
        {
            scrollViewer.Loaded -= ScrollViewer_Loaded;
            scrollViewer.Loaded += ScrollViewer_Loaded;
            SetKeepVerticalExpanded(scrollViewer, keepExpanded);
        }

        return true;
    }

    private static void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
        DebugLog.Log($"ScrollViewer_Loaded ListView ({sender.GetHashCode()})");

        if (sender is ScrollViewer scrollViewer)
        {
            ApplyKeepExpanded(scrollViewer);
        }
    }

    private static void UpdateScrollViewerVisualStates(ScrollViewer scrollViewer, bool keepExpanded)
    {
        if (keepExpanded is true)
        {
            ChangeVisualState(scrollViewer, "MouseIndicator");
            ChangeVisualState(scrollViewer, "ScrollBarSeparatorExpanded");
            RemoveVisualStatesFromScrollViewer(scrollViewer);
        }
        else
        {
            RestoreVisualStates(scrollViewer);
            ChangeVisualState(scrollViewer, "NoIndicator");
        }
    }

    private static void ScrollViewer_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
    {
        sender.EffectiveViewportChanged -= ScrollViewer_EffectiveViewportChanged;
        ApplyKeepExpanded(sender);
    }

    private static bool ApplyKeepExpandedToNavigationView(NavigationView navigationView, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"ApplyKeepExpandedToNavigationView NavigationView ({navigationView.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        if (navigationView.FindDescendant("MenuItemsScrollViewer") is ScrollViewer menuItemsScrollViewer)
        {
            _ = ApplyKeepExpandedToScrollViewer(menuItemsScrollViewer, orientation, keepExpanded);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToUnknownTarget(FrameworkElement frameworkElement, Orientation orientation, bool keepExpanded)
    {
        DebugLog.Log($"ApplyKeepExpandedToUnknownTarget FrameworkElement ({frameworkElement.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        foreach (ScrollViewer scrollViewer in frameworkElement.FindDescendants().OfType<ScrollViewer>())
        {
            if (orientation is Orientation.Vertical)
            {
                SetKeepVerticalExpanded(scrollViewer, keepExpanded);
            }
            else
            {
                SetKeepHorizontalExpanded(scrollViewer, keepExpanded);
            }
        }

        return true;
    }

    private static void ChangeVisualState(Control control, string stateName)
    {
        if (VisualStateManager.GoToState(control, stateName, true) is bool result &&
            result is false &&
            control is ScrollBar scrollBar &&
            scrollBar.FindDescendant("Root") is FrameworkElement root)
        {
            DebugLog.Log($"ChangeVisualState Control: {control.GetType().Name} ({control.GetHashCode()}) / State: {stateName} / Result: {result}");

            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                DebugLog.Log($"VisualStateGroup: {visualStateGroup.Name}");

                foreach (VisualState visualState in visualStateGroup.States)
                {
                    DebugLog.Log($"VisualState: {visualState.Name}");
                }
            }
        }
    }

    private static void RemoveVisualStatesFromScrollBar(ScrollBar scrollBar)
    {
        DebugLog.Log($"RemoveVisualStatesFromScrollBar ScrollBar ({scrollBar.GetHashCode()})");

        if (scrollBar.FindDescendant("Root") is FrameworkElement root)
        {
            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                visualStateGroup.CurrentStateChanged -= VisualStateGroup_CurrentStateChanged;
                visualStateGroup.CurrentStateChanged += VisualStateGroup_CurrentStateChanged;
            }

            RemoveVisualState(root, "ScrollingIndicatorStates", "TouchIndicator");
            RemoveVisualState(root, "ScrollingIndicatorStates", "NoIndicator");
            RemoveVisualState(root, "ConsciousStates", "Collapsed");
            RemoveVisualState(root, "ConsciousStates", "CollapsedWithoutAnimation");
        }
    }

    private static void RemoveVisualStatesFromScrollViewer(ScrollViewer scrollViewer)
    {
        DebugLog.Log($"RemoveVisualStatesFromScrollViewer ScrollViewer ({scrollViewer.GetHashCode()})");

        if (scrollViewer.FindDescendant("Root") is FrameworkElement root)
        {
            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                visualStateGroup.CurrentStateChanged -= VisualStateGroup_CurrentStateChanged;
                visualStateGroup.CurrentStateChanged += VisualStateGroup_CurrentStateChanged;
            }

            RemoveVisualState(root, "ScrollingIndicatorStates", "TouchIndicator");
            RemoveVisualState(root, "ScrollingIndicatorStates", "NoIndicator");
            RemoveVisualState(root, "ScrollBarSeparatorStates", "ScrollBarSeparatorCollapsed");
            RemoveVisualState(root, "ScrollBarSeparatorStates", "ScrollBarSeparatorCollapsedWithoutAnimation");
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

            DebugLog.Log($"RemoveVisualState {root.GetType().Name} ({root.GetHashCode()}) VisualStateGroup: {visualStateGroupName} / VisualStateName: {visualStateName} / RemovedVisualStates: {RemovedVisualStates.Count}");
        }
    }

    private static void RestoreVisualStates(FrameworkElement frameworkElement)
    {
        if (frameworkElement.FindDescendant("Root") is FrameworkElement root)
        {
            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                IEnumerable<RemovingVisualState> restoring = RemovedVisualStates
                    .Where(x => x.VisualStateGroup == visualStateGroup);

                foreach (VisualState visualState in restoring.Select(x => x.VisualState))
                {
                    visualStateGroup.States.Add(visualState);
                    DebugLog.Log($"RestoreVisualStates FrameworkElement: {frameworkElement.GetType().Name} ({frameworkElement.GetHashCode()}) / Root: {root.GetType().Name} ({root.GetHashCode()}) VisualStateGroup: {visualStateGroup.Name} / VisualStateName: {visualState.Name}");
                }

                foreach (RemovingVisualState item in restoring.ToList())
                {
                    _ = RemovedVisualStates.Remove(item);
                }
            }
        }

        DebugLog.Log($"RestoreVisualStates FrameworkElement: {frameworkElement.GetType().Name} ({frameworkElement.GetHashCode()}) RemovedVisualStates: {RemovedVisualStates.Count}");
    }
}