using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.ViewModels.Login;

namespace LenayGG_Movil.Views.Login;

public partial class SignIn : ContentPage
{
	public SignIn(ILogin login)
	{
		InitializeComponent();
		BindingContext = new SignInViewModel(Navigation, login);
	}
}