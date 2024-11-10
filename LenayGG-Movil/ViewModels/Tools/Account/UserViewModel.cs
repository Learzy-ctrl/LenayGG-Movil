using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Login;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Tools.Account
{
    public class UserViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private IServiceProvider _serviceProvider;
        public UserViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LogOutCommand = new AsyncRelayCommand(LogOut);
        }

        #region Methods
        private async Task LogOut()
        {
            var response = await DisplayAlert("Cerrar sesion", "¿Estas seguro de cerrar sesion?", "SI", "NO");
            if (response)
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                var signIn = _serviceProvider.GetService<SignIn>();
                await SecureStorage.SetAsync("Email", "");
                await SecureStorage.SetAsync("Password", "");
                await SecureStorage.SetAsync("Token", "");
                Application.Current.MainPage = new NavigationPage(signIn);
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Commands
        public ICommand LogOutCommand { get; private set; }
        #endregion
    }
}
