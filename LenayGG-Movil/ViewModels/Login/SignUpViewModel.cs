using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Login
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly ILogin _login;
        public ICommand RegisterCommand { get; private set; }
        public SignUpViewModel(ILogin login)
        {
            _login = login;
            RegisterCommand = new AsyncRelayCommand(Register);
        }

        #region Variables
        private string userName;
        private string password;
        private string email;
        private DateTime birthDate;
        private bool isLoading;
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
        #endregion

        #region Functions
        private async Task Register()
        {
            IsLoading = true;
            var response = await _login.SignUp(UserName, BirthDate, Email, Password);
           if (response.NumError != 0)
            {
                await DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
            }
            else
            {
                await DisplayAlert("Exito", "Se ha registrado el usuario correctamente", "OK");
            }
            IsLoading = false;
        }
        #endregion
    }
}
