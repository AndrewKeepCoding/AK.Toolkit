using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AK.Toolkit.Uwp.Localization
{
    public partial class Localizer
    {
        private readonly UIElementChildrenGetters _childrenGetters = new UIElementChildrenGetters();

        public bool TryRegisterUIElementChildrenGetters(Type type, Func<UIElement, IEnumerable<UIElement>> func)
        {
            return _childrenGetters.TryAdd(type, func);
        }

        private void RegisterDefaultUIElementChildrenGetters()
        {
            _childrenGetters.TryAdd(typeof(Panel), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is Panel panel)
                {
                    foreach (UIElement element in panel.Children.OfType<UIElement>())
                    {
                        children.Add(element);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(ItemsControl), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is ItemsControl itemsControl)
                {
                    foreach (UIElement element in itemsControl.Items.OfType<UIElement>())
                    {
                        children.Add(element);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(ContentControl), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if ((parent as ContentControl)?.Content is UIElement element)
                {
                    children.Add(element);
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(Border), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if ((parent as Border)?.Child is UIElement element)
                {
                    children.Add(element);
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(ContentPresenter), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if ((parent as ContentPresenter)?.Content is UIElement element)
                {
                    children.Add(element);
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(ItemsPresenter), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();
                return children;
            });

            _childrenGetters.TryAdd(typeof(Viewbox), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if ((parent as Viewbox)?.Child is UIElement element)
                {
                    children.Add(element);
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(UserControl), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if ((parent as UserControl)?.Content is UIElement element)
                {
                    children.Add(element);
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(MenuBar), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is MenuBar menuBar)
                {
                    foreach (UIElement element in menuBar.Items.OfType<UIElement>())
                    {
                        children.Add(element);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(MenuBarItem), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is MenuBarItem menuBarItem)
                {
                    foreach (UIElement element in menuBarItem.Items.OfType<UIElement>())
                    {
                        children.Add(element);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(CommandBar), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is CommandBar commandBar)
                {
                    foreach (UIElement element in commandBar.PrimaryCommands.OfType<UIElement>())
                    {
                        children.Add(element);
                    }

                    foreach (UIElement element in commandBar.SecondaryCommands.OfType<UIElement>())
                    {
                        children.Add(element);
                    }

                    if (commandBar.Content is UIElement content)
                    {
                        children.Add(content);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(NavigationView), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is NavigationView navigationView)
                {
                    foreach (UIElement element in navigationView.MenuItems.OfType<UIElement>())
                    {
                        children.Add(element);
                    }

                    if (navigationView.Header is UIElement header)
                    {
                        children.Add(header);
                    }

                    if (navigationView.PaneHeader is UIElement paneHeader)
                    {
                        children.Add(paneHeader);
                    }

                    if (navigationView.PaneFooter is UIElement paneFooter)
                    {
                        children.Add(paneFooter);
                    }

                    if (navigationView.SettingsItem is UIElement settingsItem)
                    {
                        children.Add(settingsItem);
                    }

                    if (navigationView.Content is UIElement content)
                    {
                        children.Add(content);
                    }
                }

                return children;
            });

            _childrenGetters.TryAdd(typeof(NavigationViewItem), (parent) =>
            {
                HashSet<UIElement> children = new HashSet<UIElement>();

                if (parent is NavigationViewItem navigationViewItem)
                {
                    foreach (UIElement element in navigationViewItem.GetChildren())
                    {
                        children.Add(element);
                    }

                    if (navigationViewItem.Content is UIElement content)
                    {
                        children.Add(content);
                    }
                }

                return children;
            });
        }

        private class UIElementChildrenGetters
        {
            private Dictionary<Type, Func<UIElement, IEnumerable<UIElement>>> _dictionary = new Dictionary<Type, Func<UIElement, IEnumerable<UIElement>>>();

            public bool TryAdd(Type type, Func<UIElement, IEnumerable<UIElement>> func)
            {
                if (_dictionary.TryAdd(type, func) is true)
                {
                    _dictionary = _dictionary
                        .OrderByDescending(x => x.Key.GetHierarchyFromUIElement().Count())
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                    return true;
                }

                return false;
            }

            public bool TryGetValue(Type type, out Func<UIElement, IEnumerable<UIElement>> func)
            {
                foreach (Type key in _dictionary.Keys)
                {
                    if (key.IsAssignableFrom(type) is true)
                    {
                        func = _dictionary[key];
                        return true;
                    }
                }

                if (_dictionary.TryGetValue(type, out Func<UIElement, IEnumerable<UIElement>> value) is true)
                {
                    func = value;
                    return true;
                }

                func = null;
                return false;
            }
        }
    }
}