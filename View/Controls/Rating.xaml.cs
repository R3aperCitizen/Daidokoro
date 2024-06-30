using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class Rating : ContentView
{
	private const string DATE_FORMAT = "dd/MM/yyyy HH:mm";
	private const string POSITIVE_VOTE = "🍋‍🟩";
	private const string NEGATIVE_VOTE = "🧅";

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