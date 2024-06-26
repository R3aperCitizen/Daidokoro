using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipeFullList : ContentView
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(List<Ricetta>), typeof(RecipeFullList), new List<Ricetta>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeFullList)bindable;
        control.list.ItemsSource = (List<Ricetta>)newValue;
        control.switcher.Switch = true;
    }
    public List<Ricetta> Source
    {
        get => (List<Ricetta>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<Ricetta>>), typeof(RecipeFullList), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeFullList)bindable;
        var asyncSourceTask = (Task<List<Ricetta>>)newValue;

        control.switcher.Switch = false;
        control.Source = await asyncSourceTask;
        control.switcher.Switch = true;
    }
    public Task<List<Ricetta>> AsyncSource
    {
        get => (Task<List<Ricetta>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }

    public RecipeFullList()
    {
        InitializeComponent();
    }
}