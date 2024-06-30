using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Controls
{
    [ContentProperty(nameof(Target))]
    public class AsyncView<DataType, ViewType> : ContentView where ViewType : Microsoft.Maui.Controls.View, new()
    {
        private SwitchView switcher;

        public static readonly BindableProperty TaskProperty =
            BindableProperty.Create(nameof(Task), typeof(Task<DataType>), typeof(AsyncView<DataType, ViewType>), propertyChanged: OnAsyncSourceChanged);
        private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AsyncView<DataType, ViewType>)bindable;
            control.switcher.Index = 0;
            await control.Task;
            control.switcher.Index = 1;
        }
        public Task<DataType> Task
        {
            get => (Task<DataType>)GetValue(TaskProperty);
            set => SetValue(TaskProperty, value);
        }

        public ViewType Target
        {
            get => (ViewType)switcher.Elements[1];
            set => switcher.Elements[1] = value;
        }

        public AsyncView()
        {
            switcher = new SwitchView();
            switcher.Elements.Add(new ActivityIndicator { IsRunning = true });
            switcher.Elements.Add(new ViewType());
            switcher.Index = 0;

            Content = switcher;
        }
    }
}
