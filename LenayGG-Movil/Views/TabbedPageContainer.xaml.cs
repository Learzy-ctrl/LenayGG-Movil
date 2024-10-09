using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace LenayGG_Movil.Views;

public partial class TabbedPageContainer : Microsoft.Maui.Controls.TabbedPage
{
	public TabbedPageContainer()
	{
		InitializeComponent();
		On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android >().SetIsSmoothScrollEnabled(true);
    }
}