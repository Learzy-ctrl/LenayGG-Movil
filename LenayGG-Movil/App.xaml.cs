

using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;

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

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderLessEntry), (handler, view) =>
            {
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(PickerUnderLineColor), (handler, view) =>
            {
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePickerUnderlineColor), (handler, view) =>
            {
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            });
        }
    }
}
