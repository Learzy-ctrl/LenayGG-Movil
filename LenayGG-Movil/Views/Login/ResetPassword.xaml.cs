using LenayGG_Movil.ViewModels.Login;

namespace LenayGG_Movil.Views.Login;

public partial class ResetPassword : ContentPage
{
	public ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
	{
		InitializeComponent();
		resetPasswordViewModel._navigation = this.Navigation;
		BindingContext = resetPasswordViewModel;
	}
}