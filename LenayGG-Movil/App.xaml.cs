

using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using LenayGG_Movil.Views.Wallet;

namespace LenayGG_Movil
{
    public partial class App : Application
    {
        public App(ILogin login)
        {
            InitializeComponent();
            //MainPage = new NavigationPage(new SignIn(login));
            MainPage = new EditarBilleteraCredito();

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
