using Daidokoro.Model;
using System.Diagnostics;

namespace Daidokoro.View.Controls;

public partial class RecipeMinimalList : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(List<Ricetta>), typeof(RecipeMinimalList), new List<Ricetta>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimalList)bindable;
        control.list.ItemsSource = control.Source;
        control.switcher.Index = 1;
    }
    public List<Ricetta> Source
	{
		get => (List<Ricetta>)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

    public RecipeMinimalList()
	{
		InitializeComponent();
	}

    public async void SetSourceAsync(Task<List<Ricetta>> asyncSource)
    {
        switcher.Index = 0;
        Source = await asyncSource;
        switcher.Index = 1;
    }
}