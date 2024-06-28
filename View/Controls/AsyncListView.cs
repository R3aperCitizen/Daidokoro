using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public class AsyncListView<T> : AsyncView<List<T>, CollectionView>
{
    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<T>>), typeof(AsyncListView<T>), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AsyncListView<T>)bindable;
        control.Task = control.AsyncSource;
        control.AsyncObject.ItemsSource = await control.Task;
    }
    public Task<List<T>> AsyncSource
    {
        get => (Task<List<T>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }

    public DataTemplate Template
    {
        get => AsyncObject.ItemTemplate;
        set => AsyncObject.ItemTemplate = value;
    }

    public AsyncListView() : base(new CollectionView()) { }
}