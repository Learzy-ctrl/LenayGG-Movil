using CommunityToolkit.Maui;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Services;
using LenayGG_Movil.ViewModels.Login;
using LenayGG_Movil.ViewModels.Main;
using LenayGG_Movil.ViewModels.Tools;
using LenayGG_Movil.ViewModels.Tools.Account;
using LenayGG_Movil.ViewModels.Tools.Reports;
using LenayGG_Movil.ViewModels.Wallet;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using LenayGG_Movil.Views.Main.Inicio;
using LenayGG_Movil.Views.Main.Transacciones;
using LenayGG_Movil.Views.Tools;
using LenayGG_Movil.Views.Tools.Account;
using LenayGG_Movil.Views.Tools.Reports;
using LenayGG_Movil.Views.Wallet;
using Syncfusion.Maui.Core.Hosting;
using The49.Maui.BottomSheet;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using Microsoft.Extensions.Logging;
using Plugin.Firebase.Crashlytics;
using Microsoft.Maui.LifecycleEvents;
#if IOS
using Plugin.Firebase.Bundled.Platforms.iOS;
#else
using Plugin.Firebase.Bundled.Platforms.Android;
#endif
namespace LenayGG_Movil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var ApiUrl = "https://lenaygg-backend.onrender.com/";
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBottomSheet()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddHttpClient<ILogin, Login>(client =>
            {
                client.BaseAddress = new Uri(ApiUrl);
            });

            builder.Services.AddHttpClient<IWalletInfraestructure, WalletService>(client =>
            {
                client.BaseAddress = new Uri(ApiUrl);
            });

            builder.Services.AddHttpClient<ITransactionInfraestructure, TransactionService>(client =>
            {
                client.BaseAddress = new Uri(ApiUrl);
            });

            builder.Services.AddHttpClient<IUserInfrastructure, UserService>(client =>
            {
                client.BaseAddress = new Uri(ApiUrl);
            });

            builder.Services.AddHttpClient<IReportInfrastructure, ReportService>(client =>
            {
                client.BaseAddress = new Uri(ApiUrl);
            });

            // Sign In
            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignIn>();

            // Sign Up
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignUp>();

            builder.Services.AddTransient<ResetPassword>();
            builder.Services.AddTransient<ResetPasswordViewModel>();

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

            //Inicio
            builder.Services.AddTransient<InicioLayout>();
            builder.Services.AddTransient<InicioViewModel>();

            //Transacciones
            builder.Services.AddTransient<TransaccionLayOut>();
            builder.Services.AddTransient<TransaccionesViewModel>();
            builder.Services.AddTransient<CategoriaBottomViewModel>();
            builder.Services.AddTransient<CategoriaBottomSheet>();

            //tools
            builder.Services.AddTransient<Tools>();
            builder.Services.AddTransient<ToolsViewModel>();

            //Account
            builder.Services.AddTransient<UserPage>();
            builder.Services.AddTransient<UserViewModel>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<ChangePasswordViewModel>();

            //Reports
            builder.Services.AddTransient<ReportPage>();
            builder.Services.AddTransient<ReportViewModel>();
            return builder.Build();
        }
        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                CrossFirebase.Initialize(CreateCrossFirebaseSettings());
                return false;
            }));
#else
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                    CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
#endif
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true,
            isCloudMessagingEnabled: true, isAnalyticsEnabled: true);
        }
    }
}
