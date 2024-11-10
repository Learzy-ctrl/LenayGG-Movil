using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.ViewModels.Login;

namespace LenayGG_Movil.Views.Login;

public partial class SignUp : ContentPage
{
	public SignUp(SignUpViewModel signUpViewModel)
	{
		InitializeComponent();
		signUpViewModel._navigation = this.Navigation;
		BindingContext = signUpViewModel;
	}
}