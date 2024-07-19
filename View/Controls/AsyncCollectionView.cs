using System.Collections.ObjectModel;

namespace Daidokoro.View.Controls;

public class AsyncCollectionView : ContentView
{
    private SwitchView switchView;
    private CollectionView collectionView;

    public ObservableCollection<object> Source { get; private set; }
    public Func<Task<IEnumerable<object>>>? Supplier { get; set; }
    public DataTemplate Template
    {
        get => collectionView.ItemTemplate;
        set => collectionView.ItemTemplate = value;
    }

    public AsyncCollectionView()
    {
        Source = new ObservableCollection<object>();

        collectionView = new CollectionView();
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, nameof(Source));
        collectionView.BindingContext = this;
        
        switchView = new SwitchView();
        switchView.Elements.Add(new ActivityIndicator { IsRunning = true });
        switchView.Elements.Add(collectionView);

        switchView.Index = 0;
        Content = switchView;
    }

    public async void Refresh()
    {
        if (Supplier is not null)
        {
            switchView.Index = 0;

            Source.Clear();
            foreach(var item in await Supplier.Invoke())
                Source.Add(item);

            switchView.Index = 1;
        }
    }

    // Faster, but causes problems when using Images
    public void CachedRefresh()
    {
        if (Source.Count.Equals(0))
            Refresh();
    }
}