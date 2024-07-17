using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class ObjectiveMinimalList : ContentView
{
    public static readonly BindableProperty SourceProperty =
      BindableProperty.Create(nameof(Source), typeof(List<Obiettivo>), typeof(CollectionMinimalList),propertyChanged:sourceChanged);

    private static void sourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var main = (ObjectiveMinimalList)bindable;
        main.collectionView.ItemsSource = (List<Obiettivo>)newValue;
    }

    public List<Obiettivo> Source
	{
		get => (List<Obiettivo>)GetValue(SourceProperty);
		set => SetValue(SourceProperty,value); 
	}
    public ObjectiveMinimalList()
	{
		InitializeComponent();
		
	}

}