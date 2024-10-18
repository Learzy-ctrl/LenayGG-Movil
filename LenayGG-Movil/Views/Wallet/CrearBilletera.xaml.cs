using LenayGG_Movil.ViewModels.Wallet;

namespace LenayGG_Movil.Views.Wallet
{
    public partial class CrearBilletera : ContentPage
    {
        public CrearBilletera(CrearBilleteraViewModel crearBilleteraViewModel)
        {
            InitializeComponent();
            crearBilleteraViewModel._navigation = this.Navigation;
            BindingContext = crearBilleteraViewModel;
        }
    }
}

