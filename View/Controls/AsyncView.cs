using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.View.Controls
{
    [ContentProperty(nameof(AsyncObject))]
    public class AsyncView<AsyncDataType, AsyncObjectType> : ContentView where AsyncObjectType : Microsoft.Maui.Controls.View
    {
        private SwitchView switcher;

        public static readonly BindableProperty TaskProperty =
            BindableProperty.Create(nameof(Task), typeof(Task<AsyncDataType>), typeof(AsyncView<AsyncDataType, AsyncObjectType>), propertyChanged: OnAsyncSourceChanged);
        private static async void OnAsyncSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AsyncView<AsyncDataType, AsyncObjectType>)bindable;
            control.switcher.Index = 0;
            await control.Task;
            control.switcher.Index = 1;
        }
        public Task<AsyncDataType> Task
        {
            get => (Task<AsyncDataType>)GetValue(TaskProperty);
            set => SetValue(TaskProperty, value);
        }

        public AsyncObjectType AsyncObject
        {
            get => (AsyncObjectType)switcher.Elements[1];
            set => switcher.Elements[1] = value;
        }

        public AsyncView(AsyncObjectType asyncObject)
        {
            switcher = new SwitchView();
            switcher.Elements.Add(new ActivityIndicator { IsRunning = true });
            switcher.Elements.Add(asyncObject);
            switcher.Index = 0;

            Content = switcher;
        }
    }
}
