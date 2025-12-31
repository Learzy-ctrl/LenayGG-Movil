using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Mensajes;
using LenayGG_Movil.Models.UserModel;
using LenayGG_Movil.Views.Tools.Account;
using LenayGG_Movil.Views.Tools.Reports;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Tools
{
    public class ToolsViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserInfrastructure _userInfrastructure;
        private readonly ILogin _login;
        public ToolsViewModel(IServiceProvider serviceProvider, IUserInfrastructure userInfrastructure, ILogin login)
        {
            _serviceProvider = serviceProvider;
            _userInfrastructure = userInfrastructure;
            _login = login;
            GoToAccountCommand = new AsyncRelayCommand(GoToAccount);
            GoToReportsCommand = new AsyncRelayCommand(GoToReports);
            WeakReferenceMessenger.Default.Register<ToolsMessage>(this, (r, message) =>
            {
                GetUser();
            });
            GetUser();
        }

        #region Variables
        private string _photoUrl;
        private string _userName;
        private UserDto _userDto;
        #endregion

        #region Objects
        public string PhotoUrl
        {
            get { return _photoUrl; }
            set { SetValue(ref _photoUrl, value); }
        }
        public string UserName
        {
            get { return _userName; }
            set { SetValue(ref _userName, value); }
        }
        #endregion

        #region Methods
        private async Task GoToAccount()
        {
            var userPage = _serviceProvider.GetService<UserPage>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(userPage);
            UserDialogs.Instance.HideLoading();
        }

        private async Task GoToReports()
        {
            var reportPage = _serviceProvider.GetService<ReportPage>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(reportPage);
            UserDialogs.Instance.HideLoading();
        }

        private async void GetUser()
        {
            bool sen = false;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                if (!string.IsNullOrEmpty(token))
                {
                    _userDto = await _userInfrastructure.GetUserAsync(token);
                    if (_userDto.NombreUser == "Error de conexion")
                    {
                        await DisplayAlert("Error", "Conectate internet para continuar", "OK");
                        sen = false;
                    }
                    if (_userDto.NombreUser == "Token expirado")
                    {
                        var email = SecureStorage.GetAsync("Email").Result;
                        var password = SecureStorage.GetAsync("Password").Result;
                        var result = await _login.SignIn(email, password);
                        await SecureStorage.SetAsync("Token", result.Resultado);
                        sen = true;
                    }
                    else
                    {
                        PhotoUrl = _userDto.FotoUser != null ? _userDto.FotoUser : "user";
                        UserName = _userDto.NombreUser + " " + _userDto.ApellidoUsuario;
                        sen = false;
                    }
                }
            } while (sen);
        }
        #endregion

        #region Commands
        public ICommand GoToAccountCommand { get; private set; }
        public ICommand GoToReportsCommand { get; private set; }
        #endregion
    }
}
