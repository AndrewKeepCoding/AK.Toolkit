using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3.Localization;

public partial class Localizer
{
    private readonly UIElementChildrenGetters childrenGetters = new();

    private void RegisterDefaultUIElementChildrenGetters()
    {
        _ = this.childrenGetters.TryAdd(typeof(Panel), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is Panel panel)
            {
                foreach (UIElement element in panel.Children.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ItemsControl), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is ItemsControl itemsControl)
            {
                foreach (UIElement element in itemsControl.Items.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ContentControl), (parent) =>
        {
            HashSet<UIElement> children = new();

            if ((parent as ContentControl)?.Content is UIElement element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(Border), (parent) =>
        {
            HashSet<UIElement> children = new();

            if ((parent as Border)?.Child is UIElement element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ContentPresenter), (parent) =>
        {
            HashSet<UIElement> children = new();

            if ((parent as ContentPresenter)?.Content is UIElement element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ItemsPresenter), (parent) =>
        {
            HashSet<UIElement> children = new();
            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(Viewbox), (parent) =>
        {
            HashSet<UIElement> children = new();

            if ((parent as Viewbox)?.Child is UIElement element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(UserControl), (parent) =>
        {
            HashSet<UIElement> children = new();

            if ((parent as UserControl)?.Content is UIElement element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(MenuBar), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is MenuBar menuBar)
            {
                foreach (UIElement element in menuBar.Items.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(MenuBarItem), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is MenuBarItem menuBarItem)
            {
                foreach (UIElement element in menuBarItem.Items.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(CommandBar), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is CommandBar commandBar)
            {
                foreach (UIElement element in commandBar.PrimaryCommands.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }

                foreach (UIElement element in commandBar.SecondaryCommands.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }

                if (commandBar.Content is UIElement content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(NavigationView), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is NavigationView navigationView)
            {
                foreach (UIElement element in navigationView.MenuItems.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }

                if (navigationView.AutoSuggestBox is UIElement autoSuggestBox)
                {
                    _ = children.Add(autoSuggestBox);
                }

                if (navigationView.Header is UIElement header)
                {
                    _ = children.Add(header);
                }

                if (navigationView.PaneHeader is UIElement paneHeader)
                {
                    _ = children.Add(paneHeader);
                }

                if (navigationView.PaneFooter is UIElement paneFooter)
                {
                    _ = children.Add(paneFooter);
                }

                if (navigationView.SettingsItem is UIElement settingsItem)
                {
                    _ = children.Add(settingsItem);
                }

                if (navigationView.Content is UIElement content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(NavigationViewItem), (parent) =>
        {
            HashSet<UIElement> children = new();

            if (parent is NavigationViewItem navigationViewItem)
            {
                foreach (UIElement element in navigationViewItem.MenuItems.OfType<UIElement>())
                {
                    _ = children.Add(element);
                }

                if (navigationViewItem.Content is UIElement content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });
    }

    private class UIElementChildrenGetters
    {
        private Dictionary<Type, Func<UIElement, IEnumerable<UIElement>>> dictionary = new();

        public bool TryAdd(Type type, Func<UIElement, IEnumerable<UIElement>> func)
        {
            if (this.dictionary.TryAdd(type, func) is true)
            {
                this.dictionary = this.dictionary
                    .OrderByDescending(x => x.Key.GetHierarchyFromUIElement().Count())
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                return true;
            }

            return false;
        }

        public bool TryGetValue(Type type, out Func<UIElement, IEnumerable<UIElement>>? func)
        {
            foreach (Type key in this.dictionary.Keys)
            {
                if (key.IsAssignableFrom(type) is true)
                {
                    func = this.dictionary[key];
                    return true;
                }
            }

            if (this.dictionary.TryGetValue(type, out Func<UIElement, IEnumerable<UIElement>>? value) is true)
            {
                func = value;
                return true;
            }

            func = null;
            return false;
        }
    }
}