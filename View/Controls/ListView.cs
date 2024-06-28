using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public class ListView<T> : ContentView
{
    private CollectionView list;
    private SwitchView switcher;
    private DataTemplate data;

    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(List<T>), typeof(ListView<T>), new List<T>(), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ListView<T>)bindable;
        control.list.ItemsSource = control.Source;
    }
    public List<T> Source
    {
        get => (List<T>)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly BindableProperty TemplateProperty =
        BindableProperty.Create(nameof(Template), typeof(ITemplate<T>), typeof(ListView<T>), propertyChanged: OnTemplateChanged);
    private static void OnTemplateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ListView<T>)bindable;
        control.data.LoadTemplate = () => control.Template;
    }
    public ITemplate<T> Template
    {
        get => (ITemplate<T>)GetValue(TemplateProperty);
        set => SetValue(TemplateProperty, value);
    }

    public ListView()
	{
        data = new DataTemplate(typeof(T));
        data.LoadTemplate = () => Template;

        list = new CollectionView();
        list.ItemTemplate = data;

        switcher = new SwitchView();
        switcher.Elements.Add(new ActivityIndicator { IsRunning = true });
        switcher.Elements.Add(list);

        Content = switcher;
        switcher.Index = 1;
	}

    public async void SetSourceAsync(Task<List<T>> asyncSource)
    {
        //switcher.Index = 0;
        Source = await asyncSource;
        //switcher.Index = 1;
    }
}