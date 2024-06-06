using Microsoft.Extensions.Logging;
using Daidokoro.View;
using Daidokoro.ViewModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Daidokoro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("Daidokoro.appsettings.json");
            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                            .AddJsonStream(stream)
                            .Build();

                builder.Configuration.AddConfiguration(config);
            }

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

            builder.Services.AddSingleton<IConnectivity>((e) => Connectivity.Current);
            builder.Services.AddSingleton<IMainViewModel, MainViewModel>();
            builder.Services.AddTransient<RicettaPageViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<UserPage>();
            builder.Services.AddTransient<BrowsePage>();
            builder.Services.AddTransient<RicettaPage>();
            builder.Services.AddTransient<RecipesListPage>();
            builder.Services.AddTransient<CollectionsListPage>();

            return builder.Build();
        }
    }
}
