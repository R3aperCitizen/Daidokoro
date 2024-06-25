namespace Daidokoro.View.Controls;

public partial class GenericList : ContentView
{
	public static readonly BindableProperty TemplateProperty =
		BindableProperty.Create(nameof(Template), typeof(ContentView), typeof(GenericList));
	public ContentView Template
	{
		get => (ContentView)GetValue(TemplateProperty);
		set => SetValue(TemplateProperty, value);
	}

	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(List<object>), typeof(GenericList));
	public List<object> Source
	{
		get => (List<object>)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public GenericList()
	{
		InitializeComponent();
	}
}