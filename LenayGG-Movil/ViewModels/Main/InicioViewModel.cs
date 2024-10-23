using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Main.Inicio;
using LenayGG_Movil.Views.Main.Transacciones;
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
        public INavigation _navigation {  get; set; }
        private readonly IServiceProvider _serviceProvider;
        public InicioViewModel(IServiceProvider serviceProvider)
        {
            UnifyWalletsCommand = new AsyncRelayCommand(UnifyWallets);
            NextWalletCommand = new AsyncRelayCommand(NextWallet);
            GoToTransaccionCommand = new AsyncRelayCommand(GoToTransaccion);
            _serviceProvider = serviceProvider;
        }


        #region Variables
        private View _content;
        #endregion

        #region Objects
        public View Content
        {
            get { return _content; }
            set { SetValue(ref _content, value); }
        }
        #endregion

        #region Methods

        private async Task UnifyWallets()
        {
            Content = new NoHistory();
        }

        private async Task NextWallet()
        {
            Content = new ExistHistory();
        }

        private async Task GoToTransaccion()
        {
            var transaccionLayOut = _serviceProvider.GetService<TransaccionLayOut>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(transaccionLayOut);
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Commands
        public ICommand UnifyWalletsCommand { get; private set; }
        public ICommand NextWalletCommand { get; private set; }
        public ICommand GoToTransaccionCommand { get; private set; }
        #endregion
    }
}
