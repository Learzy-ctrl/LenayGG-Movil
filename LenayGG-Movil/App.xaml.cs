

using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views.Login;

namespace LenayGG_Movil
{
    public partial class App : Application
    {
        public App(ILogin login)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SignIn(login));
        }
    }
}
