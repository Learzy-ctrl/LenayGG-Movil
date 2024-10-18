using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Wallet;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Wallet
{
    public class WalletsViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IWalletInfraestructure _wallet;
        private readonly ILogin _login;
        public ICommand GoToCreateWalletCommand { get; private set; }
        public ICommand RefreshPageCommand { get; private set; }
        public ICommand GoToDetailWalletCommand { get; private set; }
        public WalletsViewModel(IWalletInfraestructure wallet, ILogin login, IServiceProvider serviceProvider)
        {
            _wallet = wallet;
            _login = login;
            GoToCreateWalletCommand = new AsyncRelayCommand(GoToCreateWallet);
            RefreshPageCommand = new AsyncRelayCommand(RefreshView);
            GoToDetailWalletCommand = new AsyncRelayCommand<WalletDto>(GoToDetailWallet);
            _serviceProvider = serviceProvider;
            RefreshView();
        }

        #region Variables
        private bool _isrefreshing;
        private View _contentView;
        private List<WalletDto> _walletList;
        #endregion

        #region Objects
        public bool isRefreshing
        {
            get { return _isrefreshing; }
            set { SetValue(ref _isrefreshing, value); }
        }

        public View ContentView
        {
            get { return _contentView; }
            set { SetValue(ref _contentView, value); }
        }

        public List<WalletDto> WalletList
        {
            get { return _walletList; }
            set { SetValue(ref _walletList, value); }
        }
        #endregion

        #region Methods
        private async Task GoToCreateWallet()
        {
            var crearBilletera = _serviceProvider.GetService<CrearBilletera>();
            await _navigation.PushModalAsync(crearBilletera);
        }

        private async Task RefreshView()
        {
            isRefreshing = true;
            var list = await GetWallets();
            var first = list.FirstOrDefault();
            if( list.Count == 0)
            {
                ContentView = new NoWallets();
            }
            else if (first.Nombre == "Connection failure")
            {
                ContentView = new NoConnection();
            }
            else
            {
                WalletList = list;
                ContentView = new ExistWallets();
            }
            isRefreshing = false;
        }

        private async Task<List<WalletDto>> GetWallets()
        {
            bool sen = false;
            List<WalletDto> list = null;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _wallet.GetWallets(token);
                var apiResponse = response as ApiResponseDto;
                if(apiResponse != null)
                {
                    if (apiResponse.Resultado == "Connection failure")
                    {
                        var listWallet = new List<WalletDto>();
                        var wallet = new WalletDto
                        {
                            Nombre = "Connection failure"
                        };
                        listWallet.Add(wallet);
                        list = listWallet;
                        sen = false;
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
                    list = response as List<WalletDto>;
                    sen = false;
                }
            } while (sen);
            return list;
        }

        private async Task GoToDetailWallet(WalletDto walletDto)
        {
            var id = walletDto.Id;
            if(walletDto.IdTipoCuenta == 1)
            {
                var detalleCredito = _serviceProvider.GetService<DetalleCredito>();
                detalleCredito.IdBilletera = id;
                UserDialogs.Instance.ShowLoading("Cargando");
                await _navigation.PushModalAsync(detalleCredito);
            }
            else
            {
                var detalleOtros = _serviceProvider.GetService<DetalleOtros>();
                detalleOtros.IdBilletera = id;
                UserDialogs.Instance.ShowLoading("Cargando");
                await _navigation.PushModalAsync(detalleOtros);
            }
        }
        #endregion
    }
}
