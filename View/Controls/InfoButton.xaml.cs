namespace Daidokoro.View.Controls;

public partial class InfoButton : ContentView
{
    public static readonly BindableProperty ValueTextProperty = BindableProperty.Create(nameof(ValueText), typeof(string), typeof(InfoButton), propertyChanged: OnValueTextChanged);

    public string InfoText
    {
        get => infoLabel.Text;
        set => infoLabel.Text = value;
    }
    public string ValueText
    {
        get => (string)GetValue(ValueTextProperty);
        set => SetValue(ValueTextProperty, value);
    }
    public event EventHandler Clicked
    {
        add => button.Clicked += value;
        remove => button.Clicked -= value;
    }

    public InfoButton()
	{
		InitializeComponent();
	}

    private static void OnValueTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (InfoButton)bindable;
        var value = (string)newValue;

        control.valueLabel.Text = value;
    }
}