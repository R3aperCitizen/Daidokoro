using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipesList : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(List<Ricetta>), typeof(RecipesList), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipesList)bindable;

        control.loading.IsVisible = true;
        control.list.ItemsSource = (List<Ricetta>)newValue;
        control.loading.IsVisible = false;
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

        control.loading.IsVisible = true;
        await asyncSourceTask;
        control.list.ItemsSource = asyncSourceTask.Result;
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
        loading.IsVisible = false;
	}
}