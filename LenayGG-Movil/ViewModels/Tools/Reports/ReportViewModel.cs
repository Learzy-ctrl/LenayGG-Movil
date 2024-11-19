using Acr.UserDialogs;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.ReportModel;
using LenayGG_Movil.Models.WalletModel;
using LenayGG_Movil.Views.Tools.Reports;
using System.Collections.ObjectModel;

namespace LenayGG_Movil.ViewModels.Tools.Reports
{
    public class ReportViewModel : BaseViewModel
    {
        public INavigation _navigation { get; set; }
        private readonly IServiceProvider _serviceProvider;
        private readonly IReportInfrastructure _reportInfrastructure;
        private readonly IWalletInfraestructure _walletInfraestructure;
        public ObservableCollection<string> Opciones { get; } = new ObservableCollection<string>
        {
            "a Hoy", "Esta semana", "Este mes", "Este año"
        };
        public ReportViewModel(IReportInfrastructure reportInfrastructure, IWalletInfraestructure walletInfraestructure, IServiceProvider serviceProvider)
        {
            _reportInfrastructure = reportInfrastructure;
            _walletInfraestructure = walletInfraestructure;
            _serviceProvider = serviceProvider;
            GetWallets();
        }

        #region Variables
        private List<WalletDto> _walletList;
        private WalletDto _wallet;
        private string _opcionSeleccionada = "Esta semana";
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private List<GastosPorCategoriaDto> _gastos;
        private View _contentView;
        private List<Brush> _customBrushes;
        private decimal _total;
        #endregion

        #region Objects
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
                GetReports();
            }
        }

        public string OpcionSeleccionada
        {
            get { return _opcionSeleccionada; }
            set { SetValue(ref _opcionSeleccionada, value);
                GetReports();
            }
        }

        public DateTime  FechaInicio
        {
            get { return _fechaInicio; }
            set { SetValue(ref _fechaInicio, value); }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { SetValue(ref _fechaFin, value); }
        }

        public List<GastosPorCategoriaDto> Gastos
        {
            get { return _gastos; }
            set { SetValue(ref _gastos, value); }
        }

        public View ContentView
        {
            get { return _contentView; }
            set { SetValue(ref _contentView, value); }
        }
        public List<Brush> CustomBrushes
        {
            get { return _customBrushes; }
            set { SetValue(ref _customBrushes, value); }
        }

        public decimal Total
        {
            get { return _total; }
            set { SetValue(ref _total, value); }
        }
        #endregion

        #region Methods

        private async void GetWallets()
        {
            var token = SecureStorage.GetAsync("Token").Result;
            var response = await _walletInfraestructure.GetWallets(token);
            var apiResponse = response as ApiResponseDto;
            if (apiResponse != null)
            {
                if(apiResponse.NumError == 3)
                {
                    await DisplayAlert("Error", "Ups, ocurrio un problema, conectate a internet", "OK");
                    await _navigation.PopModalAsync();
                }
                else if (apiResponse.NumError == 1)
                {
                    await DisplayAlert("Error", "Ups, ocurrio un problema, intentalo mas tarde", "OK");
                    await _navigation.PopModalAsync();
                }
            }
            else
            {
                var listWallet = response as List<WalletDto>;
                var walletDto = new WalletDto();
                walletDto.Nombre = "Todas las billeteras";
                listWallet.Add(walletDto);
                WalletList = listWallet;
                Wallet = WalletList.LastOrDefault();
            }
        }

        private void ObtenerRango(string opcion)
        {
            DateTime ahora = DateTime.Now;

            switch (opcion)
            {
                case "a Hoy":
                    FechaInicio = ahora.Date; // Inicio del día
                    FechaFin = ahora.Date.AddDays(1).AddTicks(-1); // Fin del día
                    break;

                case "Esta semana":
                    int diasDesdeInicioSemana = (int)ahora.DayOfWeek;
                    FechaInicio = ahora.Date.AddDays(-diasDesdeInicioSemana); // Inicio de la semana
                    FechaFin = FechaInicio.AddDays(7).AddTicks(-1); // Fin de la semana
                    break;

                case "Este mes":
                    FechaInicio = new DateTime(ahora.Year, ahora.Month, 1); // Primer día del mes
                    FechaFin = FechaInicio.AddMonths(1).AddTicks(-1); // Último día del mes
                    break;

                case "Este año":
                    FechaInicio = new DateTime(ahora.Year, 1, 1); // Primer día del año
                    FechaFin = new DateTime(ahora.Year + 1, 1, 1).AddTicks(-1); // Último día del año
                    break;

                default:
                    throw new ArgumentException("Opción no válida");
            }
        }

        private async Task GetReports()
        {
            Total = 0;
            ObtenerRango(OpcionSeleccionada);
            var aggregate = new ConsultaGastosAggregate()
            {
                fecha_inicio = FechaInicio,
                fecha_fin = FechaFin
            };
            if(Wallet.Nombre != "Todas las billeteras")
            {
                aggregate.todas_billeteras = false;
                aggregate.id_billetera = Wallet.Id;
            }
            else
            {
                aggregate.todas_billeteras = true;
            }
            var token = await SecureStorage.GetAsync("Token");
            UserDialogs.Instance.ShowLoading("Cargando");
            var reportDto = await _reportInfrastructure.GetReports(aggregate, token) as List<GastosPorCategoriaDto>;
            if(reportDto == null)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "Ups, ocurrio un error, intenta mas tarde", "OK");
                return;
            }
            Gastos = reportDto;
            SetBrushes();
            Total = Gastos.Sum(g => g.TotalGasto);
            UserDialogs.Instance.HideLoading();
            ContentView = new CollectionCartegory();
        }

        private void SetBrushes()
        {
            CustomBrushes = new List<Brush>();
            foreach (var g in Gastos)
            {
                CustomBrushes.Add(CreateGradientFromHex(g.ColorCategoria));
            }
        }

        private LinearGradientBrush CreateGradientFromHex(string hexColor)
        {
            Color baseColor = Color.FromArgb(hexColor);

            // Derivar un color más claro
            Color lighterColor = Color.FromRgba(
                Math.Min(baseColor.Red + 30, 255),
                Math.Min(baseColor.Green + 30, 255),
                Math.Min(baseColor.Blue + 30, 255),
                baseColor.Alpha);

            // Crear el gradiente
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 0, Color = baseColor },
                new GradientStop() { Offset = 1, Color = lighterColor }
            };

            return gradient;
        }
        #endregion

        #region Commands
        #endregion
    }
}
