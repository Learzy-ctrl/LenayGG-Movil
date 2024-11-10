using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models.UserModel;
using LenayGG_Movil.Views.Login;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Tools.Account
{
    public class UserViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserInfrastructure _userInfrastructure;
        private readonly ILogin _login;
        public UserViewModel(IServiceProvider serviceProvider, IUserInfrastructure userInfrastructure, ILogin login)
        {
            _serviceProvider = serviceProvider;
            _userInfrastructure = userInfrastructure;
            _login = login;
            LogOutCommand = new AsyncRelayCommand(LogOut);
            GoBackCommand = new AsyncRelayCommand(GoBack);
            SelectPhotoCommand = new AsyncRelayCommand(SelectPhoto);
            DeleteUserCommand = new AsyncRelayCommand(DeleteUser);
            GetUser();
        }

        #region Variables
        private string _photoUrl;
        private string _userName;
        private string _userLastName;
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
        public string UserLastName
        {
            get { return _userLastName; }
            set { SetValue(ref _userLastName, value); }
        }
        #endregion

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

        private async Task GoBack()
        {
            var validate = await ValidateField();
            if (validate)
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                await UpdateUser();
                await _navigation.PopModalAsync();
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void GetUser()
        {
            bool sen;
            UserDialogs.Instance.ShowLoading("Cargando");
            do
            {

                var token = SecureStorage.GetAsync("Token").Result;
                _userDto = await _userInfrastructure.GetUserAsync(token);
                if (_userDto.NombreUser == "Error de conexion")
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Conectate internet para continuar", "OK");
                    sen = false;
                    await _navigation.PopModalAsync();
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
                    UserName = _userDto.NombreUser;
                    UserLastName = _userDto.ApellidoUsuario != null ? _userDto.ApellidoUsuario : "";
                    sen = false;
                    UserDialogs.Instance.HideLoading();
                }
            } while (sen);
        }

        private async Task SelectPhoto()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result == null)
                {
                    return;
                }

                UserDialogs.Instance.ShowLoading("Cargando imagen");
                using var stream = await result.OpenReadAsync();
                var SelectedImage = ImageSource.FromStream(() => stream);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var photoProfile = new PhotoUserAggregate
                {
                    ImageBase64 = Convert.ToBase64String(imageBytes)
                };

                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _userInfrastructure.UploadImageAsync(photoProfile, token);

                if (response.NumError != 0)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
                    return;
                }
                PhotoUrl = response.Resultado;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
            }
        }

        private async Task<bool> UpdateUser()
        {
            if (UserName != _userDto.NombreUser || UserLastName != _userDto.ApellidoUsuario)
            {
                var aggregate = new UpdateUserAggregate
                {
                    Nombre = UserName,
                    Apellido = UserLastName
                };
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _userInfrastructure.UpdateUserAsync(aggregate, token);
                if (response.NumError != 0)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private async Task<bool> ValidateField()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                await DisplayAlert("Campo vacio", "No dejes el campo nombre vacio", "OK");
                return false;
            }
            return true;
        }

        private async Task DeleteUser()
        {
            var sen = await DisplayAlert("Eliminar Cuenta", "¿Estas seguro de eliminar tu cuenta? no habra vuelta atras", "Si", "No");
            if (sen)
            {
                UserDialogs.Instance.ShowLoading("Eliminando cuenta");
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _userInfrastructure.DeleteUserAsync(token);
                if (response.NumError != 0)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Ups, Ha ocurrido un error, intentalo mas tarde", "OK");
                }
                else
                {
                    var signIn = _serviceProvider.GetService<SignIn>();
                    await SecureStorage.SetAsync("Email", "");
                    await SecureStorage.SetAsync("Password", "");
                    await SecureStorage.SetAsync("Token", "");
                    UserDialogs.Instance.HideLoading();
                    Application.Current.MainPage = new NavigationPage(signIn);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand LogOutCommand { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand SelectPhotoCommand {  get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        #endregion
    }
}
