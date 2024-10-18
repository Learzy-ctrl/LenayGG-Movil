using Acr.UserDialogs;
using Android.Media.TV;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Wallet
{
    public class DetalleOtrosViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        
        private readonly IWalletInfraestructure _wallet;
        private readonly ILogin _login;
        private readonly IServiceProvider _serviceProvider;
        public ICommand GoToEditarBilleteraCommand { get; private set; }
        public ICommand DeleteWalletCommand { get; private set; }
        public ICommand ReturnPageCommand { get; private set; }

        public DetalleOtrosViewModel(IWalletInfraestructure wallet, ILogin login, IServiceProvider serviceProvider)
        {
            _wallet = wallet;
            _login = login;
            _serviceProvider = serviceProvider;
            GoToEditarBilleteraCommand = new AsyncRelayCommand(GoToEditarBilletera);
            DeleteWalletCommand = new AsyncRelayCommand(DeleteWallet);
            ReturnPageCommand = new AsyncRelayCommand(ReturnPage);
        }

        #region Variables
        private WalletDto _walletDto;
        private Guid _idBilletera;
        #endregion

        #region Objects
        public WalletDto WalletDto
        {
            get { return _walletDto; }
            set { SetValue(ref _walletDto, value); }
        }
        public Guid IdBilletera 
        { 
            get { return _idBilletera; } 
            set { SetValue(ref _idBilletera, value);
                if(_idBilletera != Guid.Empty)
                {
                    GetWallet();
                };
            }
        }
        #endregion

        #region Methods

        private async Task GoToEditarBilletera()
        {
            var id = WalletDto.Id;
            if (WalletDto.IdTipoCuenta == 1)
            {
                var editarBilleteraCredito = _serviceProvider.GetService<EditarBilleteraCredito>();
                editarBilleteraCredito.IdBilletera = id;
                UserDialogs.Instance.ShowLoading("Cargando");
                await _navigation.PushModalAsync(editarBilleteraCredito);
            }
            else
            {
                var editarBilletera = _serviceProvider.GetService<EditarBilletera>();
                editarBilletera.IdBilletera = id;
                UserDialogs.Instance.ShowLoading("Cargando");
                await _navigation.PushModalAsync(editarBilletera);
            }
        }

        private async Task DeleteWallet()
        {
            bool option = await DisplayAlert("Confirmación", "¿Deseas eliminar esta billetera?", "Sí", "No");
            bool sen = false;
            var idWallet = new IdWalletDto
            {
                id = IdBilletera
            };
            if (option)
            {
                UserDialogs.Instance.ShowLoading("Eliminando");
                do
                {
                    var token = SecureStorage.GetAsync("Token").Result;
                    var response = await _wallet.DeleteWallet(idWallet, token);
                    if (response.Resultado == "Connection failure")
                    {
                        await DisplayAlert(response.Resultado, "Ocurrio un error, intentalo mas tarde", "Ok");
                        sen = false;
                        UserDialogs.Instance.HideLoading();
                    }
                    else if (response.NumError != 0)
                    {
                        var email = SecureStorage.GetAsync("Email").Result;
                        var password = SecureStorage.GetAsync("Password").Result;
                        var result = await _login.SignIn(email, password);
                        await SecureStorage.SetAsync("Token", result.Resultado);
                        sen = true;
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Exito", "Se ha eliminado correctamente", "ok");
                        sen = false;
                        await _navigation.PopModalAsync();
                    }
                } while (sen);
            }
        }

        private async Task ReturnPage()
        {
            await _navigation.PopModalAsync();
        }

        public async Task GetWallet()
        {
            bool sen = false;
            var idWallet = new IdWalletDto
            {
                id = IdBilletera
            };
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _wallet.GetWalletById(idWallet, token);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
                {
                    if (apiResponse.Resultado == "Connection failure")
                    {
                        await DisplayAlert(apiResponse.Resultado, "Ocurrio un error, intentalo mas tarde", "Ok");
                        await _navigation.PopModalAsync();
                        sen = false;
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var email = SecureStorage.GetAsync("Email").Result;
                        var password = SecureStorage.GetAsync("Password").Result;
                        var result = await _login.SignIn(email, password);
                        await SecureStorage.SetAsync("Token", result.Resultado);
                        sen = true;
                    }
                }
                else
                {
                    WalletDto = response as WalletDto;
                    sen = false;
                    UserDialogs.Instance.HideLoading();
                }
            } while (sen);
            
        }
        #endregion

    }
}
