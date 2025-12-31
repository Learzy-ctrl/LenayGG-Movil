using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using System.Net.Mail;
using System.Windows.Input;


namespace LenayGG_Movil.ViewModels.Login
{
    public class SignInViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly ILogin _login;
        private readonly IServiceProvider _serviceProvider;
        public ICommand GoToSignUpCommand { get; private set; }
        public ICommand GoToResetPasswordCommand { get; private set; }
        public ICommand SignInCommand {  get; private set; }
        public ICommand ViewPasswordCommand { get; private set; }
        public SignInViewModel(ILogin login, IServiceProvider serviceProvider) 
        {
            _login = login;
            _serviceProvider = serviceProvider;
            GoToSignUpCommand = new AsyncRelayCommand(GoToSignUp);
            SignInCommand = new AsyncRelayCommand(SignIn);
            GoToResetPasswordCommand = new AsyncRelayCommand(GoToResetPassword);
            ViewPasswordCommand = new RelayCommand(ViewPassword);
        }

        #region Variables
        private string email;
        private string password;
        private bool isPassword = true;
        private string imageIcon = "hide";
        #endregion

        #region Objects
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string Password
        {
            get { return password; }
            set { SetValue(ref password, value); }
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
        private async Task GoToSignUp()
        {
            var signUpPage = _serviceProvider.GetService<SignUp>();
            await _navigation.PushModalAsync(signUpPage);
        }

        private async Task GoToResetPassword()
        {
            var resetPaswword = _serviceProvider.GetService<ResetPassword>();
            await _navigation.PushModalAsync(resetPaswword);
        }

        private async Task SignIn()
        {
            var validate = await ValidateFields();
            if (validate)
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                var response = await _login.SignIn(Email, Password);
                if (response.NumError != 0)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Datos Incorrectos", "Ups, algo salio mal, intentalo mas tarde", "OK");
                }
                else
                {
                    await SecureStorage.SetAsync("Email", Email);
                    await SecureStorage.SetAsync("Password", Password);
                    await SecureStorage.SetAsync("Token", response.Resultado);
                    await _navigation.PushAsync(new PageEmpty());
                    await Task.Delay(2000);
                    var tabbedPage = _serviceProvider.GetService<TabbedPageContainer>();
                    Application.Current.MainPage = tabbedPage;
                    await Task.Delay(1400);
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private void ViewPassword()
        {
            if(IsPassword)
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

        private async Task<bool> ValidateFields()
        {
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
            if (Password.Length < 7)
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
        #endregion
    }
}
