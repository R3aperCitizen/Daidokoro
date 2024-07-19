using Microsoft.Maui.Animations;
using System.Collections;

namespace Daidokoro.View.Controls;

public class AsyncCollectionView : ContentView
{
    private SwitchView switcher;

    public Func<Task<IEnumerable>>? Supplier { get; set; }

    public DataTemplate Template
    {
        get => CollectionView.ItemTemplate;
        set => CollectionView.ItemTemplate = value;
    }

    public CollectionView CollectionView
    {
        get => (CollectionView)switcher.Elements[1];
        set => switcher.Elements[1] = value;
    }

    public AsyncCollectionView()
    {
        switcher = new SwitchView();
        switcher.Elements.Add(new ActivityIndicator { IsRunning = true });
        switcher.Elements.Add(new CollectionView());
        switcher.Index = 0;

        Content = switcher;
    }

    public async void Refresh()
    {
        if (Supplier is not null)
        {
            switcher.Index = 0;
            CollectionView.ItemsSource = await Supplier.Invoke();
            switcher.Index = 1;
        }
    }

    public void RefreshOnce()
    {
        if (CollectionView.ItemsSource is null)
            Refresh();
    }
}