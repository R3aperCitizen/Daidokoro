using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class CollectionMinimalList : ContentView
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(List<Collezione>), typeof(CollectionMinimalList), new List<Collezione>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CollectionMinimalList)bindable;
        control.list.ItemsSource = (List<Collezione>)newValue;
        control.switcher.Index = 1;
    }
    public List<Collezione> Source
    {
        get => (List<Collezione>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<Collezione>>), typeof(CollectionMinimalList), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CollectionMinimalList)bindable;
        var asyncSourceTask = (Task<List<Collezione>>)newValue;

        control.switcher.Index = 0;
        control.Source = await asyncSourceTask;
        control.switcher.Index = 1;
    }
    public Task<List<Collezione>> AsyncSource
    {
        get => (Task<List<Collezione>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }

    public CollectionMinimalList()
	{
		InitializeComponent();
	}
}