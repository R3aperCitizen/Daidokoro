using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipesList : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(List<Ricetta>), typeof(RecipesList), new List<Ricetta>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipesList)bindable;
        control.list.ItemsSource = (List<Ricetta>)newValue;
    }
    public List<Ricetta> Source
	{
		get => (List<Ricetta>)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<Ricetta>>), typeof(RecipesList), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipesList)bindable;
        var asyncSourceTask = (Task<List<Ricetta>>)newValue;
        control.Source = new List<Ricetta>();

        control.loading.IsVisible = true;
        control.Source = await asyncSourceTask;
        control.loading.IsVisible = false;
    }
    public Task<List<Ricetta>> AsyncSource
    {
        get => (Task<List<Ricetta>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }

    public RecipesList()
	{
		InitializeComponent();
        list.SetBinding(ListView.ItemsSourceProperty, nameof(Source));
	}
}