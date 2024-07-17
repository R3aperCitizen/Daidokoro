

namespace Daidokoro.View.Controls;

public partial class ObjectiveMinimal : ContentView
{
    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(ObjectiveMinimal),propertyChanged: descriptionChanged);

    private static void descriptionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var main = (ObjectiveMinimal)bindable;
        main.LabelDesc.Text = (string)newValue;
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly BindableProperty NameProperty =
           BindableProperty.Create(nameof(Name), typeof(string), typeof(ObjectiveMinimal),propertyChanged:nameChanged);

    private static void nameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var main = (ObjectiveMinimal)bindable;
        main.LabelName.Text = (string)newValue;
    }

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    public ObjectiveMinimal()
	{
		InitializeComponent();
	}
}