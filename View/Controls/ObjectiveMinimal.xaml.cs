

namespace Daidokoro.View.Controls;

public partial class ObjectiveMinimal : ContentView
{
    public static readonly BindableProperty NameProperty =
           BindableProperty.Create(nameof(Name), typeof(string), typeof(ObjectiveMinimal));
    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(ObjectiveMinimal));
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly BindableProperty ExperienceProperty =
    BindableProperty.Create(nameof(Experience), typeof(string), typeof(ObjectiveMinimal));
    public string Experience
    {
        get => (string)GetValue(ExperienceProperty);
        set => SetValue(ExperienceProperty, value);
    }

    public static readonly BindableProperty DataProperty =
    BindableProperty.Create(nameof(Data), typeof(string), typeof(ObjectiveMinimal));
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