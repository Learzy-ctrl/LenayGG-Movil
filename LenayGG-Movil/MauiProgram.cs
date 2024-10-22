using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Services;
using LenayGG_Movil.ViewModels.Login;
using LenayGG_Movil.ViewModels.Wallet;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using LenayGG_Movil.Views.Wallet;

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

            builder.Services.AddHttpClient<IWalletInfraestructure, WalletService>(client =>
            {
                client.BaseAddress = new Uri("https://lenaygg-backend.onrender.com/");
            });

            // Sign In
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignIn>();

            // Sign Up
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignUp>();

            // Wallets
            builder.Services.AddTransient<WalletsViewModel>();
            builder.Services.AddTransient<Wallets>();

            //crear billetera
            builder.Services.AddTransient<CrearBilleteraViewModel>();
            builder.Services.AddTransient<CrearBilletera>();

            //TabbedPageContainer
            builder.Services.AddTransient<TabbedPageContainer>();

            //Detalle billetera
            builder.Services.AddTransient<DetalleCredito>();
            builder.Services.AddTransient<DetalleOtros>();
            builder.Services.AddTransient<DetalleOtrosViewModel>();

            //Editar billeteras
            builder.Services.AddTransient<EditarBilletera>();
            builder.Services.AddTransient<EditarBilleteraCredito>();
            builder.Services.AddTransient<EditarBilleteraViewModel>();
            return builder.Build();
        }
    }
}
