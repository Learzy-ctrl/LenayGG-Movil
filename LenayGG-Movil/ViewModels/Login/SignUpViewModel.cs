using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using System.Net.Mail;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Login
{
    public class SignUpViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly ILogin _login;
        private readonly IServiceProvider _serviceProvider;
        public ICommand RegisterCommand { get; private set; }
        public ICommand ViewPasswordCommand { get; private set; }
        public SignUpViewModel(ILogin login, IServiceProvider serviceProvider)
        {
            _login = login;
            _serviceProvider = serviceProvider;
            RegisterCommand = new AsyncRelayCommand(Register);
            ViewPasswordCommand = new RelayCommand(ViewPassword);
        }

        #region Variables
        private string userName;
        private string password;
        private string email;
        private DateTime birthDate = DateTime.Now;
        private bool isLoading;
        private bool isPassword = true;
        private string imageIcon = "hide";
        #endregion

        #region Objects
        public string UserName
        {
            get { return userName; }
            set { SetValue(ref userName, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetValue(ref password, value); }
        }

        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { SetValue(ref birthDate, value); }
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set { SetValue(ref isLoading, value); }
        }
        public bool IsPassword
        {
            get { return isPassword; }
            set { SetValue(ref isPassword, value); }
        }
        public string ImageIcon
        {
            get { return imageIcon; }
            set { SetValue(ref imageIcon, value); }
        }
        #endregion

        #region Functions
        private async Task Register()
        {
            var validate = await ValidateFields();
            if (validate)
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                var response = await _login.SignUp(UserName, BirthDate, Email, Password);
                if (response.NumError != 0)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
                }
                else
                {
                    var response1 = await _login.SignIn(Email, Password);
                    await SecureStorage.SetAsync("Email", Email);
                    await SecureStorage.SetAsync("Password", Password);
                    await SecureStorage.SetAsync("Token", response1.Resultado);
                    await Task.Delay(2000);
                    var tabbedPage = _serviceProvider.GetService<TabbedPageContainer>();
                    Application.Current.MainPage = tabbedPage;
                    await Task.Delay(1400);
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private async Task<bool> ValidateFields()
        {
            if (string.IsNullOrEmpty(userName)) 
            {
                await DisplayAlert("Faltan campos", "No olvides llenar tu nombre", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await DisplayAlert("Campo vacio", "Ingresa tu correo electronico", "OK");
                return false;
            }
            
            if (string.IsNullOrEmpty(Password))
            {
                await DisplayAlert("Faltan campos", "No olvides crear tu contraseña", "OK");
                return false;
            }
            if(Password.Length < 7)
            {
                await DisplayAlert("Faltan campos", "La contraseña debe ser mayor a 7 caracteres", "OK");
                return false;
            }
            try
            {
                var emailDireccion = new MailAddress(Email);
                return emailDireccion.Address == Email;
            }
            catch
            {
                await DisplayAlert("Correo no valido", "Ingresa un correo electronico valido", "OK");
                return false;
            }
        }

        private void ViewPassword()
        {
            if (IsPassword)
            {
                IsPassword = false;
                ImageIcon = "view";
            }
            else
            {
                IsPassword = true;
                ImageIcon = "hide";
            }
        }
        #endregion
    }
}
