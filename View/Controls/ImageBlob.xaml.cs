
namespace Daidokoro.View.Controls;

public partial class ImageBlob : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(byte[]), typeof(ImageBlob), propertyChanged: OnSourceChanged);
    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ImageBlob)bindable;
		var value = (byte[])newValue;

		control.image.Source = ImageSource.FromStream(() => new MemoryStream(value));
    }
    public byte[] Source
	{
		get => (byte[])GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public Aspect Aspect
	{
		get => image.Aspect;
		set => image.Aspect = value;
	}

	public ImageBlob()
	{
		InitializeComponent();
	}
}