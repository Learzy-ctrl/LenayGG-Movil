using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.ViewModels.Wallet;

namespace LenayGG_Movil.Views.Wallet;

public partial class Wallets : ContentPage
{
	public Wallets(WalletsViewModel walletsViewModel)
	{
		InitializeComponent();
		walletsViewModel._navigation = this.Navigation;
		BindingContext = walletsViewModel;
	}

}