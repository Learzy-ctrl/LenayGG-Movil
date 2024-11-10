using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Main.Inicio;
using LenayGG_Movil.Views.Main.Transacciones;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Main
{
    public class InicioViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogin _login;
        private readonly ITransactionInfraestructure _transactionInfraestructure;
        private readonly IWalletInfraestructure _walletInfraestructure;
        public InicioViewModel(IServiceProvider serviceProvider, ILogin login, ITransactionInfraestructure transactionInfraestructure,
            IWalletInfraestructure walletInfraestructure)
        {
            UnifyWalletsCommand = new AsyncRelayCommand(UnifyWallets);
            NextWalletCommand = new AsyncRelayCommand(NextWallet);
            GoToTransaccionCommand = new AsyncRelayCommand(GoToTransaccion);
            RefreshTransactionsCommand = new AsyncRelayCommand(RefreshTransactions);
            _serviceProvider = serviceProvider;
            _login = login;
            _transactionInfraestructure = transactionInfraestructure;
            _walletInfraestructure = walletInfraestructure;
            _ = InitializeAsync();
        }


        #region Variables
        private View _content;
        private List<WalletDto> _walletList;
        private WalletDto _wallet;
        private List<TransaccionDto> _transaccionList;
        private string _nombreWallet;
        private decimal _diponible;
        private decimal _gastado;
        private string _colorWallet;
        private bool _isRefreshing;
        #endregion

        #region Objects
        public View Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }
        public List<WalletDto> WalletList
        {
            get { return _walletList; }
            set { SetValue(ref _walletList, value); }
        }
        public WalletDto Wallet
        {
            get { return _wallet; }
            set { SetValue(ref _wallet, value);
                NextWallet();
            }
        }

        public List<TransaccionDto> TransaccionList
        {
            get { return _transaccionList; }
            set { SetValue(ref _transaccionList, value); }
        }

        public string NombreWallet
        {
            get { return _nombreWallet; }
            set { SetValue(ref _nombreWallet, value); }
        }

        public decimal Disponible
        {
            get { return _diponible; }
            set { SetValue(ref _diponible, value); }
        }

        public decimal Gastado
        {
            get { return _gastado; }
            set { SetValue(ref _gastado, value); }
        }

        public string ColorWallet
        {
            get { return _colorWallet; }
            set { SetValue(ref _colorWallet, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetValue(ref _isRefreshing, value); }
        }
        #endregion

        #region Methods
        private async Task InitializeAsync()
        {
            var token = SecureStorage.GetAsync("Token").Result;
            if (!string.IsNullOrEmpty(token))
            {
                await Task.Delay(3000);
                await UnifyWallets();
            }
        }
        private async Task UnifyWallets()
        {
            bool isByUser = true;
            UserDialogs.Instance.ShowLoading("Cargando");
            await GetWallets();
            await GetAllTransactions(isByUser);
            UserDialogs.Instance.HideLoading();
        }
        private async Task NextWallet()
        {
            bool isByUser = false;
            UserDialogs.Instance.ShowLoading("Cargando");
            await GetAllTransactions(isByUser);
            UserDialogs.Instance.HideLoading();
        }
        private async Task GoToTransaccion()
        {
            var transaccionLayOut = _serviceProvider.GetService<TransaccionLayOut>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(transaccionLayOut);
            UserDialogs.Instance.HideLoading();
        }
        private async Task RefreshTransactions()
        {
            IsRefreshing = true;
            if(NombreWallet == "General")
            {
                await GetWallets();
                await GetAllTransactions(true);
            }
            else
            {
                await GetAllTransactions(false);
            }
            IsRefreshing = false;
        }
        private async Task GetAllTransactions(bool isByUser)
        {
            bool sen = false;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await ChooseMethodTransactions(token, isByUser);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
                {
                    if (apiResponse.Resultado == "Connection failure")
                    {
                        Content = new NoConnection();
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
                    TransaccionList = response as List<TransaccionDto>;
                    var view = TransaccionList.Count != 0 ? Content = new ExistHistory() : Content = new NoHistory();
                    SetWallet(isByUser);
                    sen = false;
                }
            } while (sen);
        }
        private async Task GetWallets()
        {
            bool sen = false;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _walletInfraestructure.GetWallets(token);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
                {
                    if (apiResponse.Resultado == "Connection failure")
                    {
                        Content = new NoConnection();
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
                    WalletList = response as List<WalletDto>;
                    sen = false;
                }
            } while (sen);
        }
        private async Task<object> ChooseMethodTransactions(string token, bool isByUser)
        {
            if (isByUser)
            {
                return await _transactionInfraestructure.GetTransaccionesByIdUsuario(token);
            }

            var idWallet = new IdWalletDto
            {
                id = Wallet.Id
            };
            return await _transactionInfraestructure.GetTransaccionesByIdWallet(idWallet, token);
        }
        private void SetWallet(bool isByUser)
        {
            if (isByUser)
            {
                NombreWallet = "General";
                Disponible = WalletList.Where(w => w.IdTipoCuenta != 1).Sum(w => w.Saldo);
                Gastado = TransaccionList.Where(t => t.TipoTransaccion == "-").Sum(t => t.Dinero);
                ColorWallet = "Black";
            }
            else
            {
                if(TransaccionList.Count != 0)
                {
                    var transaction = TransaccionList.FirstOrDefault();
                    NombreWallet = transaction.BilleteraNombre;
                    Disponible = transaction.BilleteraSaldo;
                    Gastado = TransaccionList.Where(t => t.TipoTransaccion == "-").Sum(t => t.Dinero);
                    ColorWallet = transaction.BilleteraColor;
                }
                else
                {
                    NombreWallet = Wallet.Nombre;
                    Disponible = Wallet.Saldo;
                    Gastado = 0;
                    ColorWallet = Wallet.Color;
                }
                
            }
        }

        #endregion

        #region Commands
        public ICommand UnifyWalletsCommand { get; private set; }
        public ICommand NextWalletCommand { get; private set; }
        public ICommand GoToTransaccionCommand { get; private set; }
        public ICommand RefreshTransactionsCommand { get; private set; }
        #endregion
    }
}
