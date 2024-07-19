namespace Daidokoro.View.Controls;

public partial class Rating : ContentView
{
	private const string DATE_FORMAT = "dd/MM/yyyy HH:mm";
	private const string POSITIVE_VOTE = "🍋‍🟩";
	private const string NEGATIVE_VOTE = "🧅";


    public static readonly BindableProperty ImageProperty =
		BindableProperty.Create(nameof(Image), typeof(byte[]), typeof(Rating), propertyChanged: OnImageChanged);
    private static void OnImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Rating)bindable;
		control.image.Source = (byte[])newValue;
    }
    public byte[] Image
	{
		get => (byte[])GetValue(ImageProperty);
		set => SetValue(ImageProperty, value);
	}

	public static readonly BindableProperty UsernameProperty =
		BindableProperty.Create(nameof(Username), typeof(string), typeof(Rating), propertyChanged: OnUsernameChanged);
    private static void OnUsernameChanged(BindableObject bindable, object oldValue, object newValue)
    {
		var control = (Rating)bindable;
		control.usernameLabel.Text = (string)newValue;
    }
    public string Username
	{
		get => (string)GetValue(UsernameProperty);
		set => SetValue(UsernameProperty, value);
	}

	public static readonly BindableProperty DateProperty =
		BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(Rating), propertyChanged: OnDateChanged);
    private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
		var control = (Rating)bindable;
		var date = (DateTime)newValue;
		control.dateLabel.Text = date.ToString(DATE_FORMAT);
    }
    public DateTime Date
	{
		get => (DateTime)GetValue(DateProperty);
		set => SetValue(DateProperty, value);
	}

	public static readonly BindableProperty VoteProperty =
		BindableProperty.Create(nameof(Vote), typeof(bool), typeof(Rating), propertyChanged: OnVoteChanged);
    private static void OnVoteChanged(BindableObject bindable, object oldValue, object newValue)
    {
		var control = (Rating)bindable;
		var vote = (bool)newValue;
		control.voteLabel.Text = vote ? POSITIVE_VOTE : NEGATIVE_VOTE;
    }
    public bool Vote
	{
		get => (bool)GetValue(VoteProperty);
		set => SetValue(VoteProperty, value);
	}

	public static readonly BindableProperty CommentProperty =
		BindableProperty.Create(nameof(Comment), typeof(string), typeof(Rating), propertyChanged: OnCommentChanged);
    private static void OnCommentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Rating)bindable;
		control.commentLabel.Text = (string)newValue;
    }
    public string Comment
	{
		get => (string)GetValue(CommentProperty);
		set => SetValue(CommentProperty, value);
	}

	public Rating()
	{
		InitializeComponent();
	}
}