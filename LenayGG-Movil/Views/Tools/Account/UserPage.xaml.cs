using LenayGG_Movil.ViewModels.Tools.Account;

namespace LenayGG_Movil.Views.Tools.Account;

public partial class UserPage : ContentPage
{
	public UserPage(UserViewModel userViewModel)
	{
		InitializeComponent();
		userViewModel._navigation = this.Navigation;
		BindingContext = userViewModel;
	}
}