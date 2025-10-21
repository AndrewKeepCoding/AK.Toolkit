using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

public partial class ScrollBarExtensions
{
    private static List<RemovingVisualState> RemovedVisualStates { get; } = [];

    private record RemovingVisualState(VisualStateGroup VisualStateGroup, VisualState VisualState);

    private static void OnKeepExpandedPropertyChanged(FrameworkElement keepExpandedTarget, Orientation orientation, bool keepExpanded)
    {
        Log($"OnKeepExpandedPropertyChanged FrameworkElement: {keepExpandedTarget} / Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        keepExpandedTarget.Loaded -= KeepExpandedTarget_Loaded;
        keepExpandedTarget.Loaded += KeepExpandedTarget_Loaded;

        ApplyKeepExpanded(keepExpandedTarget, orientation, keepExpanded);
    }

    private static void KeepExpandedTarget_Loaded(object sender, RoutedEventArgs e)
    {
        Log($"KeepExpandedTarget_Loaded sender: {sender}");

        if (sender is FrameworkElement target)
        {
            target.Loaded -= KeepExpandedTarget_Loaded;
            ApplyKeepExpanded(target);
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
        Log($"ApplyKeepExpandedToScrollBar {scrollBar.Orientation}ScrollBar ({scrollBar.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        if (scrollBar.Orientation == orientation)
        {
            scrollBar.EffectiveViewportChanged -= KeepExpanded_EffectiveViewportChanged;
            scrollBar.EffectiveViewportChanged += KeepExpanded_EffectiveViewportChanged;

            UpdateScrollBarVisualStates(scrollBar, keepExpanded);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToScrollViewer(ScrollViewer scrollViewer, Orientation orientation, bool keepExpanded)
    {
        Log($"ApplyKeepExpandedToScrollViewer ScrollViewer ({scrollViewer.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        scrollViewer.EffectiveViewportChanged -= KeepExpanded_EffectiveViewportChanged;
        scrollViewer.EffectiveViewportChanged += KeepExpanded_EffectiveViewportChanged;

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

    private static bool ApplyKeepExpandedToListView(ListView listView, Orientation orientation, bool keepExpanded)
    {
        Log($"ApplyKeepExpandedToListView ListView ({listView.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        if (orientation is Orientation.Vertical &&
            listView.FindDescendant("ScrollViewer") is ScrollViewer scrollViewer)
        {
            scrollViewer.Loaded -= KeepExpanded_ScrollViewer_Loaded;
            scrollViewer.Loaded += KeepExpanded_ScrollViewer_Loaded;
            SetKeepVerticalExpanded(scrollViewer, keepExpanded);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToNavigationView(NavigationView navigationView, Orientation orientation, bool keepExpanded)
    {
        Log($"ApplyKeepExpandedToNavigationView NavigationView ({navigationView.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        if (navigationView.FindDescendant("MenuItemsScrollViewer") is ScrollViewer menuItemsScrollViewer)
        {
            _ = ApplyKeepExpandedToScrollViewer(menuItemsScrollViewer, orientation, keepExpanded);
        }

        return true;
    }

    private static bool ApplyKeepExpandedToUnknownTarget(FrameworkElement frameworkElement, Orientation orientation, bool keepExpanded)
    {
        Log($"ApplyKeepExpandedToUnknownTarget FrameworkElement ({frameworkElement.GetHashCode()}) Orientation: {orientation} / KeepExpanded: {keepExpanded}");

        foreach (ScrollBar scrollBar in frameworkElement
            .FindDescendants()
            .OfType<ScrollBar>()
            .Where(x => x.Orientation == orientation))
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

    private static void KeepExpanded_ScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
        Log($"KeepExpanded_ScrollViewer_Loaded ScrollViewer ({sender.GetHashCode()})");

        if (sender is ScrollViewer scrollViewer)
        {
            ApplyKeepExpanded(scrollViewer);
        }
    }

    private static void KeepExpanded_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
    {
        sender.EffectiveViewportChanged -= KeepExpanded_EffectiveViewportChanged;
        ApplyKeepExpanded(sender);
    }

    private static void UpdateScrollBarVisualStates(ScrollBar scrollBar, bool keepExpanded)
    {
        if (keepExpanded is true)
        {
            ChangeVisualState(scrollBar, "MouseIndicator");
            ChangeVisualState(scrollBar, "Expanded");
            RemoveVisualStatesFromScrollBar(scrollBar);
        }
        else
        {
            RestoreVisualStates(scrollBar);
            ChangeVisualState(scrollBar, "Collapsed");
            ChangeVisualState(scrollBar, "NoIndicator");
        }
    }

    private static void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
    {
        Log($"VisualStateGroup_CurrentStateChanged Control: {e.Control.GetType().Name} VisualState: {e.OldState.Name} -> {e.NewState.Name}");
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

    private static void ChangeVisualState(Control control, string stateName)
    {
        if (VisualStateManager.GoToState(control, stateName, true) is bool result &&
            result is false &&
            control is ScrollBar scrollBar &&
            scrollBar.FindDescendant("Root") is FrameworkElement root)
        {
            Log($"ChangeVisualState Control: {control.GetType().Name} ({control.GetHashCode()}) / State: {stateName} / Result: {result}");

            foreach (VisualStateGroup visualStateGroup in VisualStateManager.GetVisualStateGroups(root))
            {
                Log($"VisualStateGroup: {visualStateGroup.Name}");

                foreach (VisualState visualState in visualStateGroup.States)
                {
                    Log($"VisualState: {visualState.Name}");
                }
            }
        }
    }

    private static void RemoveVisualStatesFromScrollBar(ScrollBar scrollBar)
    {
        Log($"RemoveVisualStatesFromScrollBar ScrollBar ({scrollBar.GetHashCode()})");

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
        Log($"RemoveVisualStatesFromScrollViewer ScrollViewer ({scrollViewer.GetHashCode()})");

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

            Log($"RemoveVisualState {root.GetType().Name} ({root.GetHashCode()}) VisualStateGroup: {visualStateGroupName} / VisualStateName: {visualStateName} / RemovedVisualStates: {RemovedVisualStates.Count}");
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
                    Log($"RestoreVisualStates FrameworkElement: {frameworkElement.GetType().Name} ({frameworkElement.GetHashCode()}) / Root: {root.GetType().Name} ({root.GetHashCode()}) VisualStateGroup: {visualStateGroup.Name} / VisualStateName: {visualState.Name}");
                }

                foreach (RemovingVisualState item in restoring.ToList())
                {
                    _ = RemovedVisualStates.Remove(item);
                }
            }
        }

        Log($"RestoreVisualStates FrameworkElement: {frameworkElement.GetType().Name} ({frameworkElement.GetHashCode()}) RemovedVisualStates: {RemovedVisualStates.Count}");
    }
}