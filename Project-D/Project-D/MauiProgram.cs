using Microsoft.Extensions.Logging;

namespace Project_D
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
                    fonts.AddFont("raleway.medium.ttf", "Raleway");
                    fonts.AddFont("Juicy.ttf", "JC");


                    fonts.AddFont("Brands-Regular-400.otf", "FAB");
                    fonts.AddFont("Free-Solid-900.otf", "FAS");
                    fonts.AddFont("Free-Regular-400.otf", "FAR");

                });

            builder.Services.AddSingleton<LocalDbService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            return builder.Build();
        }
    }
}
