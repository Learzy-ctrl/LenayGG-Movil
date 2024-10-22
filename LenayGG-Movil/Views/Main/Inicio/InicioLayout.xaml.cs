using LenayGG_Movil.ViewModels.Main;

namespace LenayGG_Movil.Views.Main.Inicio;

public partial class InicioLayout : ContentPage
{
	public InicioLayout(InicioViewModel inicioViewModel)
	{
		InitializeComponent();
		BindingContext = inicioViewModel;
	}
}