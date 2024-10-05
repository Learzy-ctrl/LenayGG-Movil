using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Views.Login;
using System.Windows.Input;


namespace LenayGG_Movil.ViewModels.Login
{
    public class SignInViewModel : ObservableObject
    {
        private readonly INavigation _navigation;
        private readonly ILogin _login;
        public ICommand GoToSignUpCommand { get; private set; }
        public ICommand SignInCommand {  get; private set; }
        public SignInViewModel(INavigation navigation, ILogin login) 
        {
            _navigation = navigation;
            _login = login;
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
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Functions
        private async Task GoToSignUp()
        {
            await _navigation.PushAsync(new SignUp(_login));
        }

        private async Task SignIn()
        {

        }
        #endregion
    }
}
