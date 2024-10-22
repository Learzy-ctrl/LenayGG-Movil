using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Login;
using System.Windows.Input;


namespace LenayGG_Movil.ViewModels.Login
{
    public class SignInViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly ILogin _login;
        private readonly IWalletInfraestructure _wallet;
        private readonly IServiceProvider _serviceProvider;
        public ICommand GoToSignUpCommand { get; private set; }
        public ICommand SignInCommand {  get; private set; }
        public SignInViewModel(ILogin login, IWalletInfraestructure wallet, IServiceProvider serviceProvider) 
        {
            _login = login;
            _wallet = wallet;
            _serviceProvider = serviceProvider;
            GoToSignUpCommand = new AsyncRelayCommand(GoToSignUp);
            SignInCommand = new AsyncRelayCommand(SignIn);
        }

        #region Variables
        private string email;
        private string password;
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
        #endregion

        #region Functions
        private async Task GoToSignUp()
        {
            var signUpPage = _serviceProvider.GetService<SignUp>();
            await _navigation.PushAsync(signUpPage);
        }

        private async Task SignIn()
        {
            UserDialogs.Instance.ShowLoading("Cargando");
            var response = await _login.SignIn(Email, Password);
            if(response.NumError != 0)
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
        #endregion
    }
}
