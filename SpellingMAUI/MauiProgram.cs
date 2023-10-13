using Microsoft.Extensions.Logging;

namespace SpellingMAUI
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
            builder.Services.AddSingleton<YourListPage>();
            builder.Services.AddTransient<SpellingPage>();
            //builder.Services.AddSingleton<TodoListPage>();
            //builder.Services.AddTransient<TodoItemPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ResultsPage>();
            builder.Services.AddSingleton<TodoItemDatabase>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}