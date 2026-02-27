using LocationHeatMapApp.Services;
using Microsoft.Extensions.Logging;

namespace LocationHeatMapApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ===== REGISTER SERVICES =====
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
        builder.Services.AddSingleton(new LocationDb(dbPath));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

#if DEBUG
       
#endif

        return builder.Build();
    }
}
