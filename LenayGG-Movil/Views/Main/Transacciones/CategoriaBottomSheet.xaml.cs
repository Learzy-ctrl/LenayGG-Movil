using LenayGG_Movil.ViewModels.Main;
using The49.Maui.BottomSheet;

namespace LenayGG_Movil.Views.Main.Transacciones;

public partial class CategoriaBottomSheet : BottomSheet
{
	public CategoriaBottomSheet(CategoriaBottomViewModel categoriaBottomViewModel)
	{
		InitializeComponent();
		categoriaBottomViewModel.bottomSheet = this;
		BindingContext = categoriaBottomViewModel;
	}
}