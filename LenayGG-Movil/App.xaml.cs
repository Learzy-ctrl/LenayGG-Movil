

using LenayGG_Movil.Views.Login;

namespace LenayGG_Movil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SignIn();
        }
    }
}
