using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models.LoginModel;
using LenayGG_Movil.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Tools.Account
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IUserInfrastructure _userInfrastructure;
        public ChangePasswordViewModel(IUserInfrastructure userInfrastructure)
        {
            _userInfrastructure = userInfrastructure;
            ChangePasswordCommand = new AsyncRelayCommand(ChangePassword);
            ChangeVisibilityOldPasswordCommand = new RelayCommand(ChangeVisibilityOldPassword);
            ChangeVisibilityNewPasswordCommand = new RelayCommand(ChangeVisibilityNewPassword);
            ChangeVisibilityConfirmPasswordCommand = new RelayCommand(ChangeVisibilityConfirmPassword);
            _validateOldPassword = SecureStorage.GetAsync("Password").Result;
        }

        #region Variables
        private string _validateOldPassword;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _icon = "hide";
        private bool _isPassword = true;
        private string _icon2 = "hide";
        private bool _isPassword2 = true;
        private string _icon3 = "hide";
        private bool _isPassword3 = true;
        #endregion

        #region Objects
        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetValue(ref _oldPassword, value); }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set { SetValue(ref _newPassword, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetValue(ref _confirmPassword, value); }
        }

        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetValue(ref _isPassword, value); }
        }

        public string Icon
        {
            get { return _icon; }
            set { SetValue(ref _icon, value); }
        }

        public bool IsPassword2
        {
            get { return _isPassword2; }
            set { SetValue(ref _isPassword2, value); }
        }

        public string Icon2
        {
            get { return _icon2; }
            set { SetValue(ref _icon2, value); }
        }

        public bool IsPassword3
        {
            get { return _isPassword3; }
            set { SetValue(ref _isPassword3, value); }
        }

        public string Icon3
        {
            get { return _icon3; }
            set { SetValue(ref _icon3, value); }
        }
        #endregion

        #region Methods
        private async Task ChangePassword()
        {
            bool validate = await ValidateFields();
            if (validate)
            {
                UserDialogs.Instance.ShowLoading("Guardando cambios");
                var token = SecureStorage.GetAsync("Token").Result;
                var aggregate = new PasswordAggregate
                {
                    password = NewPassword
                };
                var response = await _userInfrastructure.ChangePasswordAsync(aggregate, token);
                UserDialogs.Instance.HideLoading();
                if(response.NumError == 0)
                {
                    await SecureStorage.SetAsync("Password", NewPassword);
                    await DisplayAlert("Operacion exitosa", "Se ha cambiado tu contraseña de manera correcta", "OK");
                    await _navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Operacion fallida", "Ups, algo salio mal, intentalo mas tarde", "OK");
                    await _navigation.PopModalAsync();
                }
            }
        }
        private async Task<bool> ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                await DisplayAlert("Validacion de campos", "Debes ingresar tu contraseña actual", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await DisplayAlert("Validacion de campos", "Debes ingresar tu nueva contraseña", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await DisplayAlert("Validacion de campos", "Debes ingresar la confirmacion de la contraseña", "OK");
                return false;
            }
            if(OldPassword != _validateOldPassword)
            {
                await DisplayAlert("Validacion de campos", "Tu contraseña actual es incorrecta", "OK");
                return false;
            }
            if(NewPassword != ConfirmPassword)
            {
                await DisplayAlert("Validacion de campos", "La nueva contraseña no coincide con la confirmacion", "OK");
                return false;
            }
            return true;
        }
        private void ChangeVisibilityOldPassword()
        {
            if (IsPassword)
            {
                IsPassword = false;
                Icon = "view";
            }
            else
            {
                IsPassword = true;
                Icon = "hide";
            }
        }
        private void ChangeVisibilityNewPassword()
        {
            if (IsPassword2)
            {
                IsPassword2 = false;
                Icon2 = "view";
            }
            else
            {
                IsPassword2 = true;
                Icon2 = "hide";
            }
        }
        private void ChangeVisibilityConfirmPassword()
        {
            if (IsPassword3)
            {
                IsPassword3 = false;
                Icon3 = "view";
            }
            else
            {
                IsPassword3 = true;
                Icon3 = "hide";
            }
        }
        #endregion

        #region Commands
        public ICommand ChangePasswordCommand { get; private set; }
        public ICommand ChangeVisibilityOldPasswordCommand { get; private set; }
        public ICommand ChangeVisibilityNewPasswordCommand { get; private set; }
        public ICommand ChangeVisibilityConfirmPasswordCommand { get; private set; }
        #endregion
    }
}
