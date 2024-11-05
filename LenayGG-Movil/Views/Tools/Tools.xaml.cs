using LenayGG_Movil.ViewModels.Tools;

namespace LenayGG_Movil.Views.Tools;

public partial class Tools : ContentPage
{
	public Tools(ToolsViewModel toolsViewModel)
	{
		InitializeComponent();
		toolsViewModel._navigation = this.Navigation;
		BindingContext = toolsViewModel;
	}
}