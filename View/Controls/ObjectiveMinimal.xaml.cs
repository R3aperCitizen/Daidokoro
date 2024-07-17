

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

    public static readonly BindableProperty ExpProperty =
    BindableProperty.Create(nameof(Exp), typeof(string), typeof(ObjectiveMinimal), propertyChanged: expChanged);

    private static void expChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var main = (ObjectiveMinimal)bindable;
        main.LabelExp.Text = "XP: " + (string)newValue;
    }

    public string Exp
    {
        get => (string)GetValue(ExpProperty);
        set => SetValue(ExpProperty, value);
    }

    public static readonly BindableProperty DataProperty =
    BindableProperty.Create(nameof(Data), typeof(string), typeof(ObjectiveMinimal), propertyChanged: dataChanged);

    private static void dataChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var main = (ObjectiveMinimal)bindable;
        main.LabelData.Text = "Data: " + (string)newValue;
    }

    public string Data
    {
        get => (string)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public ObjectiveMinimal()
	{
		InitializeComponent();
	}
}