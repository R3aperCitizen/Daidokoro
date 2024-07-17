using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class Rating : ContentView
{
	public static readonly BindableProperty ModelProperty
		= BindableProperty.Create(nameof(Model), typeof(Valutazione), typeof(Rating), propertyChanged: OnModelChanged);
    private static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Rating)bindable;
        control.BindingContext = control.Model;
    }
    public Valutazione Model
    {
        get => (Valutazione)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public Rating()
	{
		InitializeComponent();
	}
}