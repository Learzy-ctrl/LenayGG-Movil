using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using LenayGG_Movil.Views.Main.Inicio;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Handlers;
using LenayGG_Movil.Views.Main.Transacciones;

#if ANDROID
using AndroidX.AppCompat.Widget;
#endif

namespace LenayGG_Movil
{
    public partial class App : Application
    {
        public App(TabbedPageContainer tabbedPageContainer, SignIn signIn)
        {
            InitializeComponent();


            var Token = SecureStorage.GetAsync("Token").Result;
            if (!string.IsNullOrEmpty(Token))
            {
                MainPage = tabbedPageContainer;
            }
            else
            {
                MainPage = new NavigationPage(signIn);
            }


#if ANDROID


            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderLessEntry), (handler, view) =>
            {
                if (handler.PlatformView is AppCompatEditText editText)
                {
                    editText.Background = null;  // Esto eliminará el fondo
                }
            });


            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(PickerUnderLineColor), (handler, view) =>
            {
                if (handler.PlatformView is AppCompatEditText editText)
                {
                    editText.Background = null;  // Esto eliminará el fondo
                }
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePickerUnderlineColor), (handler, view) =>
            {
                if (handler.PlatformView is AppCompatEditText editText)
                {
                    editText.Background = null;  // Esto eliminará el fondo
                }
            });
#endif
        }
    }
}
