using Microsoft.Extensions.Logging;
using Daidokoro.View;
using Daidokoro.ViewModel;

namespace Daidokoro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IMainViewModel, MainViewModel>();
            builder.Services.AddTransient<HomePage>();

            return builder.Build();
        }
    }
}
