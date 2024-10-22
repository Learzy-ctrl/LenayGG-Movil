using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views.Wallet;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Wallet
{
    public class CrearBilleteraViewModel : BaseViewModel
    {
        
        private readonly IWalletInfraestructure _wallet;
        private readonly ILogin _login;
        public ObservableCollection<string> Opciones { get; } = new ObservableCollection<string>
        {
            "Credito", "Debito", "Nomina", "Efectivo"
        };
        public INavigation _navigation { get; set; }
        public ICommand CreateWalletCommand { get; private set; }
        public ICommand ShowBottomSheetCommand {  get; private set; }
        public CrearBilleteraViewModel(IWalletInfraestructure wallet, ILogin login)
        {
            _wallet = wallet;
            _login = login;
            CreateWalletCommand = new AsyncRelayCommand(CreateWallet);
            ShowBottomSheetCommand = new AsyncRelayCommand(ShowBottomSheet);
        }

        #region variables
        private string _nombre;
        private int _tipoCuenta;
        private decimal _limiteCredito;
        private DateTime _fechaPago;
        private DateTime _fechaCorte;
        private decimal _tasaInteres;
        private string _color;
        private decimal _saldo;
        private string _opcionSeleccionada;
        private View _optionCard;
        #endregion

        #region Objects
        public string _Nombre
        {
            get { return _nombre; }
            set { SetValue(ref _nombre, value); }
        }
        public int _TipoCuenta
        {
            get { return _tipoCuenta; }
            set { SetValue(ref _tipoCuenta, value); }
        }
        public decimal _LimiteCredito
        {
            get { return _limiteCredito; }
            set { SetValue(ref _limiteCredito, value); }
        }
        public DateTime _FechaPago
        {
            get { return _fechaPago; }
            set { SetValue(ref _fechaPago, value); }
        }
        public DateTime _FechaCorte
        {
            get { return _fechaCorte; }
            set { SetValue(ref _fechaCorte, value); }
        }
        public decimal _TasaInteres
        {
            get { return _tasaInteres; }
            set { SetValue(ref _tasaInteres, value); }
        }
        public string _Color
        {
            get { return _color; }
            set { SetValue(ref _color, value); }
        }
        public decimal _Saldo
        {
            get { return _saldo; }
            set { SetValue(ref _saldo, value); }
        }

        public string _OpcionSeleccionada
        {
            get { return _opcionSeleccionada; }
            set { SetValue(ref _opcionSeleccionada, value);
                  ChangeView(); }
        }

        public View OptionCard
        {
            get { return _optionCard; }
            set { SetValue(ref _optionCard, value); }
        }
        #endregion

        #region methods
        private async Task CreateWallet()
        {
            var walletDto = new WalletAgregate
            {
                idBilletera = Guid.Empty,
                Nombre = _Nombre,
                IdTipoCuenta = TipoCuenta(),
                LimiteCredito = _LimiteCredito,
                FechaDePago = _FechaPago,
                FechaCorte = _FechaCorte,
                TazaInteres = _TasaInteres,
                Color = _Color,
                Saldo = _Saldo,
                IdUsuario = ""
            };
            
            bool sen = false;
            UserDialogs.Instance.ShowLoading("Cargando");
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _wallet.AddWallet(walletDto, token);
                if(response.NumError != 0)
                {
                    var email = SecureStorage.GetAsync("Email").Result;
                    var password = SecureStorage.GetAsync("Password").Result;
                    var result = await _login.SignIn(email, password);
                    await SecureStorage.SetAsync("Token", result.Resultado);
                    sen = true;
                }else if(string.IsNullOrEmpty(response.Resultado))
                {
                    sen = false;
                    await DisplayAlert("Error", "Algo malo ocurrio... intentalo mas tarde", "OK");
                }
                else
                {
                    sen = false;
                    await DisplayAlert("Exito", "Se ha creado una nueva billetera", "OK");
                }
            } while (sen);
            UserDialogs.Instance.HideLoading();
        }

        private int TipoCuenta()
        {
            int option = 0;
            switch (_OpcionSeleccionada)
            {
                case "Credito":
                    option = 1;
                    break;
                case "Debito":
                    option = 2;
                    break;
                case "Nomina":
                    option = 3;
                    break;
                case "Efectivo":
                    option = 4;
                    break;
            }
            return option;
        }
        
        private void ChangeView()
        {
            if(_OpcionSeleccionada == "Credito")
            {
                OptionCard = new OpcionesCreditoCrear();
            }
            else
            {
                OptionCard = new OpcionesOtrasCuentas();
            }
        }

        private async Task ShowBottomSheet()
        {

        }
        #endregion
    }
}
