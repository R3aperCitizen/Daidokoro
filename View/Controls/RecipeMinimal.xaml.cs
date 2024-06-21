using Daidokoro.Model;

namespace Daidokoro.View.Controls;

public partial class RecipeMinimal : ContentView
{
    private const string DIFFICULTY_SYMBOL = "🌶️";
    private const string LIKES_SYMBOL = "❤️";
    private const string TIME_UNIT = "min";

    public static readonly BindableProperty IdProperty =
        BindableProperty.Create(nameof(Id), typeof(int), typeof(RecipeMinimal));
    public new int Id
    {
        get => (int)GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    public static readonly BindableProperty TitleProperty = 
        BindableProperty.Create(nameof(Title), typeof(string), typeof(RecipeMinimal), "Loading...", propertyChanged: OnTitleChanged);
    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimal)bindable;
        control.titleLabel.Text = (string)newValue;
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty TimeProperty = 
        BindableProperty.Create(nameof(Time), typeof(int), typeof(RecipeMinimal), 0, propertyChanged: OnTimeChanged);
    private static void OnTimeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimal)bindable;
        control.timeLabel.Text = $"{(int)newValue} {TIME_UNIT}";
    }
    public int Time
    {
        get => (int)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly BindableProperty DifficultyProperty = 
        BindableProperty.Create(nameof(Difficulty), typeof(int), typeof(RecipeMinimal), 0, propertyChanged: OnDifficultyChanged);
    private static void OnDifficultyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimal)bindable;
        control.difficultyLabel.Text = Enumerable.Repeat(DIFFICULTY_SYMBOL, (int)newValue)
                                                 .Aggregate((s1, s2) => s1 + s2);
    }
    public int Difficulty
    {
        get => (int)GetValue(DifficultyProperty);
        set => SetValue(DifficultyProperty, value);
    }

    public static readonly BindableProperty LikesProperty = 
        BindableProperty.Create(nameof(Likes), typeof(int), typeof(RecipeMinimal), -1, propertyChanged: OnLikesChanged);
    private static void OnLikesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimal)bindable;
        control.likesLabel.Text = $"{(int)newValue} {LIKES_SYMBOL}";
    }
    public int Likes
    {
        get => (int)GetValue(LikesProperty);
        set => SetValue(LikesProperty, value);
    }

    public static readonly BindableProperty ImageProperty = 
        BindableProperty.Create(nameof(Image), typeof(byte[]), typeof(RecipeMinimal), propertyChanged: OnImageChanged);
    private static void OnImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (RecipeMinimal)bindable;
        control.imageBlob.Source = (byte[])newValue;
    }
    public byte[] Image
    {
        get => (byte[])GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

	public RecipeMinimal()
	{
		InitializeComponent();
	}

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RecipePage)}?IdRicetta={Id}");
    }
}