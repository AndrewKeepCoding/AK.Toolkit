using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
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
            HashSet<DependencyObject> children = new();

            if (parent is Panel panel)
            {
                foreach (DependencyObject element in panel.Children.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ItemsControl), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is ItemsControl itemsControl)
            {
                foreach (DependencyObject element in itemsControl.Items.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ContentControl), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if ((parent as ContentControl)?.Content is DependencyObject element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(Border), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if ((parent as Border)?.Child is DependencyObject element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ContentPresenter), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if ((parent as ContentPresenter)?.Content is DependencyObject element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(ItemsPresenter), (parent) =>
        {
            HashSet<DependencyObject> children = new();
            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(Viewbox), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if ((parent as Viewbox)?.Child is DependencyObject element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(UserControl), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if ((parent as UserControl)?.Content is DependencyObject element)
            {
                _ = children.Add(element);
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(MenuBar), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is MenuBar menuBar)
            {
                foreach (DependencyObject element in menuBar.Items.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(MenuBarItem), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is MenuBarItem menuBarItem)
            {
                foreach (DependencyObject element in menuBarItem.Items.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(CommandBar), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is CommandBar commandBar)
            {
                foreach (DependencyObject element in commandBar.PrimaryCommands.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }

                foreach (DependencyObject element in commandBar.SecondaryCommands.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }

                if (commandBar.Content is DependencyObject content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(NavigationView), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is NavigationView navigationView)
            {
                foreach (DependencyObject element in navigationView.MenuItems.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }

                if (navigationView.AutoSuggestBox is DependencyObject autoSuggestBox)
                {
                    _ = children.Add(autoSuggestBox);
                }

                if (navigationView.Header is DependencyObject header)
                {
                    _ = children.Add(header);
                }

                if (navigationView.PaneHeader is DependencyObject paneHeader)
                {
                    _ = children.Add(paneHeader);
                }

                if (navigationView.PaneFooter is DependencyObject paneFooter)
                {
                    _ = children.Add(paneFooter);
                }

                if (navigationView.SettingsItem is DependencyObject settingsItem)
                {
                    _ = children.Add(settingsItem);
                }

                if (navigationView.Content is DependencyObject content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(NavigationViewItem), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is NavigationViewItem navigationViewItem)
            {
                foreach (DependencyObject element in navigationViewItem.MenuItems.OfType<DependencyObject>())
                {
                    _ = children.Add(element);
                }

                if (navigationViewItem.Content is DependencyObject content)
                {
                    _ = children.Add(content);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(TextBlock), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is TextBlock textBlock)
            {
                foreach (Inline inline in textBlock.Inlines)
                {
                    _ = children.Add(inline);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(RadioButtons), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is RadioButtons radioButons)
            {
                foreach (DependencyObject item in radioButons.Items.OfType<DependencyObject>())
                {
                    _ = children.Add(item);
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(RichTextBlock), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is RichTextBlock richTextBlock)
            {
                foreach (Paragraph paragraph in richTextBlock.Blocks.OfType<Paragraph>())
                {
                    foreach (Inline inline in paragraph.Inlines)
                    {
                        _ = children.Add(inline);
                    }
                }
            }

            return children;
        });

        _ = this.childrenGetters.TryAdd(typeof(Hyperlink), (parent) =>
        {
            HashSet<DependencyObject> children = new();

            if (parent is Hyperlink hyperlink)
            {
                foreach (Inline inline in hyperlink.Inlines)
                {
                    _ = children.Add(inline);
                }
            }

            return children;
        });
    }

    private class UIElementChildrenGetters
    {
        private Dictionary<Type, Func<DependencyObject, IEnumerable<DependencyObject>>> dictionary = new();

        public bool TryAdd(Type type, Func<DependencyObject, IEnumerable<DependencyObject>> func)
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

        public bool TryGetValue(Type type, out Func<DependencyObject, IEnumerable<DependencyObject>>? func)
        {
            foreach (Type key in this.dictionary.Keys)
            {
                if (key.IsAssignableFrom(type) is true)
                {
                    func = this.dictionary[key];
                    return true;
                }
            }

            if (this.dictionary.TryGetValue(type, out Func<DependencyObject, IEnumerable<DependencyObject>>? value) is true)
            {
                func = value;
                return true;
            }

            func = null;
            return false;
        }
    }
}