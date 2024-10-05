using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.ViewModels.Login;

namespace LenayGG_Movil.Views.Login;

public partial class SignUp : ContentPage
{
	public SignUp(ILogin login)
	{
		InitializeComponent();
		BindingContext = new SignUpViewModel(login, this);
	}
}