using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Daidokoro.View.Controls;

[ContentProperty(nameof(Elements))]
public partial class SwitchView : ContentView
{
    public ObservableCollection<Microsoft.Maui.Controls.View> Elements { get; set; }

    public static readonly BindableProperty IndexProperty =
        BindableProperty.Create(nameof(Index), typeof(int), typeof(SwitchView), 0, propertyChanged: OnIndexChanged);
    private static void OnIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SwitchView)bindable;
        control.Content = control.Elements[control.Index];
    }
    public int Index
    {
        get => (int)GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }

    public SwitchView()
    {
        Elements = new();
        Elements.CollectionChanged += OnElementsChanged;
    }

    private void OnElementsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Content = Elements[Index];
    }
}