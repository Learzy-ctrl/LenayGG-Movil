using LenayGG_Movil.ViewModels.Wallet;

namespace LenayGG_Movil.Views.Wallet;

public partial class EditarBilleteraCredito : ContentPage
{
    public Guid IdBilletera
    {
        get => ((EditarBilleteraViewModel)BindingContext).IdBilletera;
        set => ((EditarBilleteraViewModel)BindingContext).IdBilletera = value;
    }
    public EditarBilleteraCredito(EditarBilleteraViewModel editarBilleteraViewModel)
	{
		InitializeComponent();
        editarBilleteraViewModel._navigation = this.Navigation;
        BindingContext = editarBilleteraViewModel;
    }
}