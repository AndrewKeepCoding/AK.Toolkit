using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;

namespace AK.Toolkit.WinUI3;

public class NumberBoxEx : NumberBox
{
    public static readonly DependencyProperty NumberHorizontalAlignmentProperty = DependencyProperty.Register(
        nameof(NumberHorizontalAlignment),
        typeof(HorizontalAlignment),
        typeof(NumberBoxEx),
        new PropertyMetadata(HorizontalAlignment.Right, (d, e) => (d as NumberBoxEx)?.UpdateNumberHorizontalAlignment()));

    public static readonly DependencyProperty IsDeleteButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsDeleteButtonVisible),
        typeof(bool),
        typeof(NumberBoxEx),
        new PropertyMetadata(true));

    public NumberBoxEx()
    {
        Loaded += NumberBoxEx_Loaded;
        Unloaded += NumberBoxEx_Unloaded;
        GotFocus += NumberBoxEx_GotFocus;
        LostFocus += NumberBoxEx_LostFocus;
    }

    public HorizontalAlignment NumberHorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(NumberHorizontalAlignmentProperty);
        set => SetValue(NumberHorizontalAlignmentProperty, value);
    }

    public bool IsDeleteButtonVisible
    {
        get => (bool)GetValue(IsDeleteButtonVisibleProperty);
        set => SetValue(IsDeleteButtonVisibleProperty, value);
    }

    private ScrollViewer? ContentElement { get; set; }

    private TextBox? InputBox { get; set; }

    private Button? DeleteButton { get; set; }

    private long DeleteButtonPropertyChangedCallbackToken { get; set; }

    private static IEnumerable<T> FindChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T targetChild)
            {
                yield return targetChild;
            }

            foreach (T grandChild in FindChildrenOfType<T>(child))
            {
                yield return grandChild;
            }
        }
    }

    private void UpdateNumberHorizontalAlignment()
    {
        if (ContentElement is not null)
        {
            ContentElement.HorizontalAlignment = NumberHorizontalAlignment;
        }
    }

    private void NumberBoxEx_Loaded(object sender, RoutedEventArgs e)
    {
        if (FindChildrenOfType<ScrollViewer>(this)
            .Where(x => x.Name == nameof(ContentElement))
            .FirstOrDefault() is ScrollViewer scrollViewer)
        {
            ContentElement = scrollViewer;
            UpdateNumberHorizontalAlignment();
        }

        if (FindChildrenOfType<TextBox>(this)
            .Where(x => x.Name == nameof(InputBox))
            .FirstOrDefault() is TextBox inputBox)
        {
            InputBox = inputBox;
            InputBox.SetBinding(
                TextBox.MinWidthProperty,
                new Binding
                {
                    Source = this,
                    Path = new PropertyPath("MinWidth"),
                    Mode = BindingMode.OneWay,
                });
        }

        if (FindChildrenOfType<Button>(this)
            .Where(x => x.Name == nameof(DeleteButton))
            .FirstOrDefault() is Button deleteButton)
        {
            DeleteButton = deleteButton;
            DeleteButtonPropertyChangedCallbackToken = DeleteButton.RegisterPropertyChangedCallback(
                VisibilityProperty,
                OnDeleteButtonVisibilityPropertyChanged);
        }
    }

    private void NumberBoxEx_Unloaded(object sender, RoutedEventArgs e)
    {
        UnregisterPropertyChangedCallback(VisibilityProperty, DeleteButtonPropertyChangedCallbackToken);
    }

    private void OnDeleteButtonVisibilityPropertyChanged(DependencyObject sender, DependencyProperty dp)
    {
        if (DeleteButton is not null && IsDeleteButtonVisible is false)
        {
            DeleteButton.Visibility = Visibility.Collapsed;
        }
    }

    private void NumberBoxEx_GotFocus(object sender, RoutedEventArgs e)
    {
        if (DeleteButton is not null)
        {
            DeleteButton.Visibility = IsDeleteButtonVisible is true
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    private void NumberBoxEx_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DeleteButton is not null)
        {
            DeleteButton.Visibility = Visibility.Collapsed;
        }
    }
}
