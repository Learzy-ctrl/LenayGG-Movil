using LenayGG_Movil.Views.Main.Inicio;
using LenayGG_Movil.Views.Wallet;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace LenayGG_Movil.Views;

public partial class TabbedPageContainer : Microsoft.Maui.Controls.TabbedPage
{
	public TabbedPageContainer(Wallets wallets, InicioLayout inicioLayout)
	{
		InitializeComponent();
		On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android >().SetIsSmoothScrollEnabled(true);

        var homePage = inicioLayout;
        homePage.Title = "Inicio";
        homePage.IconImageSource = "home";
        this.Children.Add(homePage);

		var walletPage = wallets;
        walletPage.Title = "Billeteras";
        walletPage.IconImageSource = "wallet";
        this.Children.Add(walletPage);

        var notificationPage = new Notification.Notifications();
        notificationPage.Title = "Notificaciones";
        notificationPage.IconImageSource = "ringing";
        this.Children.Add(notificationPage);

        var toolPage = new Tools.Tools();
        toolPage.Title = "Mas";
        toolPage.IconImageSource = "support";
        this.Children.Add(toolPage);
    }
}