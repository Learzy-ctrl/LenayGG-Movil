using LenayGG_Movil.ViewModels.Tools.Account;

namespace LenayGG_Movil.Views.Tools.Account;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordViewModel changePasswordViewModel)
	{
		InitializeComponent();
		changePasswordViewModel._navigation = this.Navigation;
		BindingContext = changePasswordViewModel;
	}
}