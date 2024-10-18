using LenayGG_Movil.ViewModels.Wallet;

namespace LenayGG_Movil.Views.Wallet;

public partial class EditarBilletera : ContentPage
{
    public Guid IdBilletera
    {
        get => ((EditarBilleteraViewModel)BindingContext).IdBilletera;
        set => ((EditarBilleteraViewModel)BindingContext).IdBilletera = value;
    }
    public EditarBilletera(EditarBilleteraViewModel editarBilleteraViewModel)
	{
		InitializeComponent();
        editarBilleteraViewModel._navigation = this.Navigation;
        BindingContext = editarBilleteraViewModel;
	}
}