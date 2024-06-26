using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RatingList : ContentView
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(List<Valutazione>), typeof(RatingList), new List<Valutazione>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RatingList)bindable;
        control.list.ItemsSource = (List<Valutazione>)newValue;
    }
    public List<Valutazione> Source
    {
        get => (List<Valutazione>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<Valutazione>>), typeof(RatingList), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RatingList)bindable;
        var asyncSourceTask = (Task<List<Valutazione>>)newValue;

        control.switcher.Switch = false;
        control.Source = await asyncSourceTask;
        control.switcher.Switch = true;
    }
    public Task<List<Valutazione>> AsyncSource
    {
        get => (Task<List<Valutazione>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }
    public RatingList()
	{
		InitializeComponent();
	}
}