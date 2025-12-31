using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.ViewModels.Login;

namespace LenayGG_Movil.Views.Login;

public partial class SignIn : ContentPage
{
	public SignIn(SignInViewModel signInViewModel)
	{
		InitializeComponent();
		signInViewModel._navigation = Navigation;
		BindingContext = signInViewModel;
	}
}