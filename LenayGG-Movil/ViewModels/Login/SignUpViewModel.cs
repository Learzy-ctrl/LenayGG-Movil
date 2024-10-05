

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Login
{
    public class SignUpViewModel : ObservableObject
    {
        private readonly ILogin _login;
        private readonly ContentPage _contentPage;
        public ICommand RegisterCommand { get; private set; }
        public SignUpViewModel(ILogin login, ContentPage page)
        {
            _login = login;
            _contentPage = page;
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
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
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

        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                if (birthDate != value)
                {
                    birthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Functions
        private async Task Register()
        {
            IsLoading = true;
            var response = await _login.SignUp(UserName, BirthDate, Email, Password);
            if (response.tipoError != 0)
            {
                await _contentPage.DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
            }
            else
            {
                await _contentPage.DisplayAlert("Exito", "Se ha registrado el usuario correctamente", "OK");
            }
            IsLoading = false;
        }
        #endregion
    }
}
