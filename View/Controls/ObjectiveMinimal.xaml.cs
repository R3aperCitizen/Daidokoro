using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class ObjectiveMinimal : ContentView
{
    public static readonly BindableProperty ModelProperty
        = BindableProperty.Create(nameof(Model), typeof(Obiettivo), typeof(ObjectiveMinimal), propertyChanged: OnModelChanged);
    private static void OnModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ObjectiveMinimal)bindable;
        control.BindingContext = control.Model;
    }
    public Obiettivo Model
    {
        get => (Obiettivo)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public ObjectiveMinimal()
	{
		InitializeComponent();
	}
}