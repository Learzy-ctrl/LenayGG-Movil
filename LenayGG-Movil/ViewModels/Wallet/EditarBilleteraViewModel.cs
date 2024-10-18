using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Wallet
{
    public class EditarBilleteraViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }

        private readonly IWalletInfraestructure _wallet;
        private readonly ILogin _login;
        public ICommand UpdateWalletCommand { get; private set; }

        public EditarBilleteraViewModel(IWalletInfraestructure wallet, ILogin login, IServiceProvider serviceProvider)
        {
            _wallet = wallet;
            _login = login;
            UpdateWalletCommand = new AsyncRelayCommand(UpdateWallet);
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
            set
            {
                SetValue(ref _idBilletera, value);
                if (_idBilletera != Guid.Empty)
                {
                    GetWallet();
                };
            }
        }
        #endregion

        #region Methods
        private async Task UpdateWallet()
        {
            bool sen = false;
            var walletAgregate = TransformData();
            UserDialogs.Instance.ShowLoading("Actualizando");
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _wallet.UpdateWallet(walletAgregate, token);
                if (response.NumError == 2)
                {
                    await DisplayAlert(response.Resultado, "Ocurrio un error, intentalo mas tarde", "Ok");
                    sen = false;
                    UserDialogs.Instance.HideLoading();
                }
                else if (response.NumError == 1)
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
                    await DisplayAlert("Exito", "Se ha actualizado correctamente", "ok");
                    sen = false;
                    await _navigation.PopModalAsync();
                }
            } while (sen);
        }

        private WalletAgregate TransformData()
        {
            var walletAgregate = new WalletAgregate
            {
                idBilletera = WalletDto.Id,
                Nombre = WalletDto.Nombre,
                Saldo = WalletDto.Saldo,
                LimiteCredito = WalletDto.LimiteCredito,
                TazaInteres = WalletDto.TazaInteres,
                IdTipoCuenta = WalletDto.IdTipoCuenta,
                FechaDePago = WalletDto.FechaDePago,
                FechaCorte = WalletDto.FechaCorte,
                IdUsuario = WalletDto.IdUsuario,
                Color = WalletDto.Color
            };
            return walletAgregate;
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
