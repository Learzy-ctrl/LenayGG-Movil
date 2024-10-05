using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Services;
using LenayGG_Movil.ViewModels.Login;
using LenayGG_Movil.Views.Login;

namespace LenayGG_Movil
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
            builder.Services.AddHttpClient<ILogin, Login>(client =>
            {
                client.BaseAddress = new Uri("https://lenaygg-backend.onrender.com/");
            });
            return builder.Build();
        }
    }
}
