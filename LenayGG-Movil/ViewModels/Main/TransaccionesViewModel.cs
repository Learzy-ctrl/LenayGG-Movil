using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Main.Transacciones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Main
{
    public class TransaccionesViewModel : BaseViewModel
    {
        public TransaccionesViewModel()
        {
            ChangeToGastoCommand = new RelayCommand(ChangeToGasto);
            ChangeToIngresoCommand = new RelayCommand(ChangeToIngreso);
            ChangeToTransferenciaCommand = new RelayCommand(ChangeToTransferencia);
            ChangeToGasto();
        }

        #region Variables
        private const string _gastoTitulo = "Dinero gastado";
        private const string _ingresoTitulo = "Ingresar Dinero";
        private const string _transferenciaTitulo = "Transferir a billetera";
        private string _titulo;
        private View _content;
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
        #endregion

        #region Commands
        public ICommand ChangeToGastoCommand { get; private set; }
        public ICommand ChangeToIngresoCommand { get; private set; }
        public ICommand ChangeToTransferenciaCommand { get; private set; }
        #endregion
    }
}
