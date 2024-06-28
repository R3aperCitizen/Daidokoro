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
        control.switcher.Index = 1;
    }
    public List<Ricetta> Source
    {
        get => (List<Ricetta>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public RecipeFullList()
    {
        InitializeComponent();
    }

    public async Task SetSourceAsync(Task<List<Ricetta>> asyncSource)
    {
        switcher.Index = 0;
        Source = await asyncSource;
        switcher.Index = 1;
    }
}