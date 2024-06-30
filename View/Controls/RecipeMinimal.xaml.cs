using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipeMinimal : ContentView
{
    public static readonly BindableProperty ModelProperty
        = BindableProperty.Create(nameof(Model), typeof(Ricetta), typeof(RecipeFull), propertyChanged: OnModelChanged);
    private static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeFull)bindable;
        control.BindingContext = control.Model;
    }
    public Ricetta Model
    {
        get => (Ricetta)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public RecipeMinimal()
	{
		InitializeComponent();
	}

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RecipePage)}?IdRicetta={Id}");
    }
}