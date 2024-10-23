using LenayGG_Movil.ViewModels.Main;

namespace LenayGG_Movil.Views.Main.Transacciones;

public partial class TransaccionLayOut : ContentPage
{
	public TransaccionLayOut(TransaccionesViewModel transaccionesViewModel)
	{
		InitializeComponent();
		BindingContext = transaccionesViewModel;
	}
}