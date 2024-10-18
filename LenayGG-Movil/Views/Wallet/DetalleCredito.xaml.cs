using LenayGG_Movil.ViewModels.Wallet;

namespace LenayGG_Movil.Views.Wallet;

public partial class DetalleCredito : ContentPage
{
    public Guid IdBilletera
    {
        get => ((DetalleOtrosViewModel)BindingContext).IdBilletera;
        set => ((DetalleOtrosViewModel)BindingContext).IdBilletera = value;
    }
    public DetalleCredito(DetalleOtrosViewModel detalleOtrosViewModel)
	{
		InitializeComponent();
		detalleOtrosViewModel._navigation = this.Navigation;
		BindingContext = detalleOtrosViewModel;
	}
}