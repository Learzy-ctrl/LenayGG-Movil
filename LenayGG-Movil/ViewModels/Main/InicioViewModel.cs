using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views;
using LenayGG_Movil.Views.Main.Inicio;
using LenayGG_Movil.Views.Main.Transacciones;
using LenayGG_Movil.Views.Wallet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            UnifyWalletsCommand = new RelayCommand(UnifyWallets);
            NextWalletCommand = new RelayCommand(NextWallet);
            GoToTransaccionCommand = new AsyncRelayCommand(GoToTransaccion);
            RefreshTransactionsCommand = new AsyncRelayCommand(RefreshTransactions);
            _serviceProvider = serviceProvider;
            _login = login;
            _transactionInfraestructure = transactionInfraestructure;
            _walletInfraestructure = walletInfraestructure;
            UnifyWallets();
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

        private async void UnifyWallets()
        {
            var list = await GetWallets();
            var first = list.FirstOrDefault();
            if (list.Count == 0)
            {
                Content = new NoWallets();
            }
            else if (first.Nombre == "Connection failure")
            {
                Content = new NoConnection();
            }
            else
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                WalletList = list;
                await GetAllTransactions();
                UserDialogs.Instance.HideLoading();
            }
            
        }

        private async void NextWallet()
        {
            UserDialogs.Instance.ShowLoading("Cargando");
            await GetTransactionByWallet();
            UserDialogs.Instance.HideLoading();
        }

        private async Task GoToTransaccion()
        {
            var transaccionLayOut = _serviceProvider.GetService<TransaccionLayOut>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(transaccionLayOut);
            UserDialogs.Instance.HideLoading();
        }

        private async Task GetAllTransactions()
        {
            bool sen;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _transactionInfraestructure.GetTransaccionesByIdUsuario(token);
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
                    var list = response as List<TransaccionDto>;
                    TransaccionList = list;
                    SetWalletUnify();
                    sen = false;
                }
            } while (sen);
            if (TransaccionList.Count == 0)
            {
                Content = new NoHistory();
            }
            else
            {
                Content = new ExistHistory();
            }
        }

        private async Task GetTransactionByWallet()
        {
            var idWallet = new IdWalletDto
            {
                id = Wallet.Id
            };
            bool sen;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _transactionInfraestructure.GetTransaccionesByIdWallet(idWallet, token);
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
                    var list = response as List<TransaccionDto>;
                    TransaccionList = list;
                    SetWallet();
                    Content = new ExistHistory();
                    sen = false;
                }
            } while (sen);
            if(TransaccionList.Count == 0)
            {
                Content = new NoHistory();
            }
            else
            {
                Content = new ExistHistory();
            }
        }

        private void SetWallet()
        {
            NombreWallet = Wallet.Nombre;
            if(Wallet.IdTipoCuenta == 1)
            {
                Disponible = Wallet.LimiteCredito + Wallet.Saldo;
            }
            else
            {
                Disponible = Wallet.Saldo;
            }
            Gastado = TransaccionList.Where(t => t.TipoTransaccion == "-").Sum(t => t.Dinero);
            ColorWallet = Wallet.Color;
        }

        private void SetWalletUnify()
        {
            NombreWallet = "General";
            Disponible = WalletList.Where(w => w.IdTipoCuenta  != 1).Sum(t => t.Saldo);
            Gastado = TransaccionList.Where(t => t.TipoTransaccion == "-").Sum(t => t.Dinero);
            ColorWallet = "black";
        }

        private async Task<List<WalletDto>> GetWallets()
        {
            bool sen = false;
            List<WalletDto> list = null;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _walletInfraestructure.GetWallets(token);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
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

        private async Task RefreshTransactions()
        {
            IsRefreshing = true;
            if(NombreWallet == "General")
            {
                await GetAllTransactions();
            }
            else
            {
                await GetTransactionByWallet();
            }
            IsRefreshing = false;
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
