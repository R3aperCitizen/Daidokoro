using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipesList : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(List<Ricetta>), typeof(RecipesList));
	public List<Ricetta> Source
	{
		get => (List<Ricetta>)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public RecipesList()
	{
		InitializeComponent();
	}
}