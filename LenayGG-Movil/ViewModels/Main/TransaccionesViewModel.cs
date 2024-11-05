using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views.Main.Transacciones;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Main
{
    public class TransaccionesViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly ITransactionInfraestructure _transactionInfraestructure;
        private readonly ILogin _login;
        private readonly IWalletInfraestructure _walletInfraestructure;
        public TransaccionesViewModel(IServiceProvider serviceProvider, ITransactionInfraestructure transactionInfraestructure, ILogin login, IWalletInfraestructure walletInfraestructure)
        {
            _serviceProvider = serviceProvider;
            _transactionInfraestructure = transactionInfraestructure;
            _login = login;
            _walletInfraestructure = walletInfraestructure;
            ChangeToGastoCommand = new RelayCommand(ChangeToGasto);
            ChangeToIngresoCommand = new RelayCommand(ChangeToIngreso);
            ChangeToTransferenciaCommand = new RelayCommand(ChangeToTransferencia);
            ShowCategoriesCommand = new AsyncRelayCommand(ShowCategories);
            SaveTransactionCommand = new AsyncRelayCommand(SaveTransaction);
            ChangeToGasto();
            GetWallets();

            MessagingCenter.Subscribe<CategoriaBottomViewModel, CategoriaDto>(this, "CategoriaItemSelected", (sender, categoriaItem) =>
            {
                SelectedCategoriaItem = categoriaItem;
            });
        }

        ~TransaccionesViewModel()
        {
            MessagingCenter.Unsubscribe<CategoriaBottomViewModel, ColorItem>(this, "ColorItemSelected");
        }
        
        #region Variables
        private const string _gastoTitulo = "Dinero gastado";
        private const string _ingresoTitulo = "Ingresar Dinero";
        private const string _transferenciaTitulo = "Transferir a billetera";
        private string _titulo;
        private View _content;
        private TransactionAggregate _transactionAgg;
        private TransferAggregate _transferAgg;
        private decimal _dinero;
        private string _descripcion;
        private DateTime _fecha = DateTime.Parse(DateTime.Now.ToString());
        private CategoriaDto _selectedCategoriaItem;
        private List<WalletDto> _walletList;
        private WalletDto _wallet;
        private WalletDto _wallet2;
        #endregion

        #region Objects
        public View Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { SetValue(ref _titulo, value); }
        }

        public TransactionAggregate TransactionAgg
        {
            get { return _transactionAgg; }
            set { SetValue(ref _transactionAgg, value); }
        }

        public TransferAggregate TransferAgg
        {
            get { return _transferAgg; }
            set { SetValue(ref _transferAgg, value); }
        }

        public decimal Dinero
        {
            get { return _dinero; }
            set { SetValue(ref _dinero, value); }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { SetValue(ref _descripcion, value); }
        }
        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetValue(ref _fecha, value); }
        }
        public CategoriaDto SelectedCategoriaItem
        {
            get { return _selectedCategoriaItem; }
            set
            {
                SetValue(ref _selectedCategoriaItem, value);
            }
        }
        public List<WalletDto> WalletList
        {
            get { return _walletList; }
            set { SetValue(ref _walletList, value); }
        }
        public WalletDto Wallet
        {
            get { return _wallet; }
            set
            {
                SetValue(ref _wallet, value);
            }
        }
        public WalletDto Wallet2
        {
            get { return _wallet2; }
            set
            {
                SetValue(ref _wallet2, value);
            }
        }
        #endregion

        #region Methods
        private void ChangeToGasto()
        {
            Titulo = _gastoTitulo;
            Content = new Gastar();
        }
        private void ChangeToIngreso()
        {
            Titulo = _ingresoTitulo;
            Content = new Ingresar();
        }
        private void ChangeToTransferencia()
        {
            Titulo = _transferenciaTitulo;
            Content = new Transferir();
        }
        private async Task ShowCategories()
        {
            UserDialogs.Instance.ShowLoading("Cargando");
            var bottomSheet = _serviceProvider.GetService<CategoriaBottomSheet>();
            await bottomSheet.ShowAsync();
        }
        private async Task SaveTransaction()
        {
            var typeTransaction = GetTypeTransaction();
            var validate = await ValidateFields(typeTransaction);
            if (validate)
            {
                SetValues(typeTransaction);
                await CreateTransaction(typeTransaction);
                ClearObjects();
            }
        }
        private int GetTypeTransaction()
        {
            int typeTransaction = 0;
            switch (Content)
            {
                case Gastar:
                    typeTransaction = 1;
                    break;
                case Ingresar:
                    typeTransaction = 2;
                    break;
                case Transferir:
                    typeTransaction = 3;
                    break;
            }
            return typeTransaction;
        }
        private void SetValues(int typeTransaction)
        {
            switch (typeTransaction)
            {
                case 1:
                    SetValuesGasto();
                    break;
                case 2:
                    SetValuesIngreso();
                    break;
                case 3: 
                    SetValuesTransferencia();
                    break;
            }
        } 
        private void SetValuesGasto()
        {
            var transaction = new TransactionAggregate()
            {
                Dinero = this.Dinero,
                Descripcion = this.Descripcion,
                Fecha = this.Fecha,
                IdCategoria = SelectedCategoriaItem.Id,
                IdBilletera = Wallet.Id,
                IdUsuario = ""
            };
            TransactionAgg = transaction;
        }
        private void SetValuesIngreso()
        {
            int categoriaIngreso = 21;

            var transaction = new TransactionAggregate()
            {
                Dinero = this.Dinero,
                Descripcion = this.Descripcion,
                Fecha = this.Fecha,
                IdCategoria = categoriaIngreso,
                IdBilletera = Wallet.Id,
                IdUsuario = ""
            };
            TransactionAgg = transaction;
        }
        private void SetValuesTransferencia()
        {
            int categoriaIngreso = 21;
            int categoriaGasto = 22;

            var transactionGasto = new TransactionAggregate()
            {
                Dinero = this.Dinero,
                Descripcion = this.Descripcion,
                Fecha = this.Fecha,
                IdCategoria = categoriaGasto,
                IdBilletera = Wallet.Id,
                IdUsuario = ""
            };

            var transactionIngreso = new TransactionAggregate()
            {
                Dinero = this.Dinero,
                Descripcion = this.Descripcion,
                Fecha = this.Fecha,
                IdCategoria = categoriaIngreso,
                IdBilletera = Wallet2.Id,
                IdUsuario = ""
            };
            var transfer = new TransferAggregate()
            {
                Gasto = transactionGasto,
                Ingreso = transactionIngreso
            };

            TransferAgg = transfer;
        }
        private async Task<bool> ValidateFields(int typeTransaction)
        {
            bool validate = false;
            if (Dinero == 0)
            {
                await DisplayAlert("Faltan Campos", "No olvides colocar el monto", "OK");
                return validate;
            }
            switch (typeTransaction)
            {
                case 1:
                    validate = await ValidateFieldsGasto();
                    break;
                case 2:
                    validate = await ValidateFieldsIngreso();
                    break;
                case 3:
                    validate = await ValidateFieldsTransferencia();
                    break;
            }
            return validate;
        }
        private async Task<bool> ValidateFieldsGasto()
        {
            if (string.IsNullOrEmpty(Descripcion))
            {
                await DisplayAlert("Faltan Campos", "No olvides poner la descripcion", "OK");
                return false;
            }
            else if(SelectedCategoriaItem == null)
            {
                await DisplayAlert("Faltan Campos", "No olvides seleccionar una categoria", "OK");
                return false;
            }
            else if(Wallet == null)
            {
                await DisplayAlert("Faltan Campos", "No olvides seleccionar una billetera", "OK");
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateFieldsIngreso()
        {
            if (string.IsNullOrEmpty(Descripcion))
            {
                await DisplayAlert("Faltan Campos", "No olvides poner la descripcion", "OK");
                return false;
            }
            else if (Wallet == null)
            {
                await DisplayAlert("Faltan Campos", "No olvides seleccionar una billetera", "OK");
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateFieldsTransferencia()
        {
            if (string.IsNullOrEmpty(Descripcion))
            {
                await DisplayAlert("Faltan Campos", "No olvides poner la descripcion", "OK");
                return false;
            }
            else if (Wallet == null || Wallet2 == null)
            {
                await DisplayAlert("Faltan Campos", "No olvides seleccionar una billetera", "OK");
                return false;
            }
            return true;
        }
        private async Task CreateTransaction(int typeTransaction)
        {
            bool sen = false;
            UserDialogs.Instance.ShowLoading("Enviando registro");
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await ChooseTypeTransaction(typeTransaction, token);
                if (response.NumError == 3)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert(response.Resultado, "Ups, ocurrio un error al registrar tu transaccion, intentalo de nuevo", "OK");
                    sen = false;
                }
                else if(response.NumError == 2)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", response.Resultado, "OK");
                    sen = false;
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
                    await DisplayAlert(response.Resultado, "Se ha registrado correctamente", "OK");
                    sen = false;
                }
            } while (sen);
        }
        private async Task<ApiResponseDto> ChooseTypeTransaction(int typeTransaction, string token)
        {
            switch (typeTransaction)
            {
                case 1:
                    return await _transactionInfraestructure.AddGasto(TransactionAgg, token);
                case 2:
                    return await _transactionInfraestructure.AddIngreso(TransactionAgg, token);
                case 3:
                    return await _transactionInfraestructure.AddTransferencia(TransferAgg, token);
            }
            return new ApiResponseDto
            {
                Resultado = "Error",
                NumError = 3
            };
        }
        private void ClearObjects()
        {
            Dinero = 0;
            Descripcion = "";
            Fecha = DateTime.Now;
            SelectedCategoriaItem = null;
            TransactionAgg = null;
            TransferAgg = null;
            Wallet = null;
            Wallet2 = null;
        }
        private async void GetWallets()
        {
            bool sen = false;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _walletInfraestructure.GetWallets(token);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
                {
                    if (apiResponse.NumError == 1)
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
        #endregion

        #region Commands
        public ICommand ChangeToGastoCommand { get; private set; }
        public ICommand ChangeToIngresoCommand { get; private set; }
        public ICommand ChangeToTransferenciaCommand { get; private set; }
        public ICommand ShowCategoriesCommand { get; private set; }
        public ICommand SaveTransactionCommand { get; private set; }
        #endregion
    }
}
