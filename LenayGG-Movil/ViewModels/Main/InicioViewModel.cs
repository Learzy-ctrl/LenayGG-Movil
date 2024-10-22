using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Main.Inicio;
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
        
        public InicioViewModel()
        {
            UnifyWalletsCommand = new AsyncRelayCommand(UnifyWallets);
            NextWalletCommand = new AsyncRelayCommand(NextWallet);
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
        #endregion

        #region Commands
        public ICommand UnifyWalletsCommand { get; private set; }
        public ICommand NextWalletCommand { get; private set; }
        #endregion
    }
}
