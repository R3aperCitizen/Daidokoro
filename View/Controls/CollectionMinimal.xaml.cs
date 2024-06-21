namespace Daidokoro.View.Controls;

public partial class CollectionMinimal : ContentView
{
    public static readonly BindableProperty IdProperty =
        BindableProperty.Create(nameof(Id), typeof(int), typeof(CollectionMinimal));
    public new int Id
    {
        get => (int)GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    public static readonly BindableProperty ImageProperty =
        BindableProperty.Create(nameof(Image), typeof(byte[]), typeof(CollectionMinimal), propertyChanged: OnImageChanged);
    private static void OnImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CollectionMinimal)bindable;
        control.imageBlob.Source = (byte[])newValue;
    }
    public byte[] Image
    {
        get => (byte[])GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CollectionMinimal), propertyChanged: OnTitleChanged);
    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CollectionMinimal)bindable;
        control.titleLabel.Text = (string)newValue;
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty CategoryProperty =
        BindableProperty.Create(nameof(Category), typeof(string), typeof(CollectionMinimal), propertyChanged: OnCategoryChanged);
    private static void OnCategoryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CollectionMinimal)bindable;
        control.categoryLabel.Text = (string)newValue;
    }
    public string Category
    {
        get => (string)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

	public CollectionMinimal()
	{
		InitializeComponent();

        titleLabel.Text = "null";
        categoryLabel.Text = "null";
	}

    private async void GoToCollectionDietPage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(CollectionDietPage)}?IdCollezione={Id}");
    }
}