using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public class AsyncListView<ModelType, TemplateType> : AsyncView<List<ModelType>, CollectionView> where TemplateType : Microsoft.Maui.Controls.View, new()
{
    public static readonly BindableProperty AsyncSourceProperty =
        BindableProperty.Create(nameof(AsyncSource), typeof(Task<List<ModelType>>), typeof(AsyncListView<ModelType, TemplateType>), propertyChanged: OnAsyncSourceChanged);
    private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AsyncListView<ModelType, TemplateType>)bindable;
        control.Task = control.AsyncSource;
        control.Target.ItemsSource = await control.Task;
    }
    public Task<List<ModelType>> AsyncSource
    {
        get => (Task<List<ModelType>>)GetValue(AsyncSourceProperty);
        set => SetValue(AsyncSourceProperty, value);
    }

    public AsyncListView()
    {
        this.Target.ItemTemplate = new DataTemplate(typeof(ModelType));
        this.Target.ItemTemplate.LoadTemplate = () => new TemplateType();
    }
}