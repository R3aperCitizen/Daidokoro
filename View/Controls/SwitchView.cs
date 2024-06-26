namespace Daidokoro.View.Controls;

public partial class SwitchView : ContentView
{
    public static readonly BindableProperty PrimaryProperty =
        BindableProperty.Create(nameof(Primary), typeof(Microsoft.Maui.Controls.View), typeof(SwitchView), propertyChanged: OnPrimaryChanged);
    private static void OnPrimaryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SwitchView)bindable;
        control.Content = control.Switch ? control.Secondary : control.Primary;
    }
    public Microsoft.Maui.Controls.View Primary
    {
        get => (Microsoft.Maui.Controls.View)GetValue(PrimaryProperty);
        set => SetValue(PrimaryProperty, value);
    }

    public static readonly BindableProperty SecondaryProperty =
        BindableProperty.Create(nameof(Secondary), typeof(Microsoft.Maui.Controls.View), typeof(SwitchView), propertyChanged: OnSecondaryChanged);
    private static void OnSecondaryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SwitchView)bindable;
        control.Content = control.Switch ? control.Secondary : control.Primary;
    }
    public Microsoft.Maui.Controls.View Secondary
    {
        get => (Microsoft.Maui.Controls.View)GetValue(SecondaryProperty);
        set => SetValue(SecondaryProperty, value);
    }

    public static readonly BindableProperty SwitchProperty =
        BindableProperty.Create(nameof(Switch), typeof(bool), typeof(SwitchView), false, propertyChanged: OnSwitchChanged);
    private static void OnSwitchChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SwitchView)bindable;
        control.Content = control.Switch ? control.Secondary : control.Primary;
    }
    public bool Switch
    {
        get => (bool)GetValue(SwitchProperty);
        set => SetValue(SwitchProperty, value);
    }
}