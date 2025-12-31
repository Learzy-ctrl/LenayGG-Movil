using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models.LoginModel;
using System.Net.Mail;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Login
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly ILogin _login;
        public ICommand ResetPasswordCommand { get; private set; }
        public ResetPasswordViewModel(ILogin login)
        {
            _login = login;
            ResetPasswordCommand = new AsyncRelayCommand(ResetPassword);
        }

        #region Variables
        private string email;
        #endregion

        #region Objects
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        #endregion

        #region Functions

        private async Task ResetPassword()
        {
            var validate = await ValidateEmail();
            if (validate)
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                var aggregate = new ResetPasswprdAggregate()
                {
                    Email = this.Email
                };
                var response = await _login.ResetPasswordByEmail(aggregate);
                if (response.NumError != 0)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Ups, algo salio mal, intentalo mas tarde", "OK");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Correo enviado", "Se ha enviado un correo a tu email, sigue las instrucciones para restablecer tu contraseña", "OK");
                    await _navigation.PopModalAsync();
                }
            }
        }

        private async Task<bool> ValidateEmail()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await DisplayAlert("Campo vacio", "Ingresa tu correo electronico", "OK");
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
