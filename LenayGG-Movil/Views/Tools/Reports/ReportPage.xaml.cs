using LenayGG_Movil.ViewModels.Tools.Reports;
using System.Collections.ObjectModel;

namespace LenayGG_Movil.Views.Tools.Reports;

public partial class ReportPage : ContentPage
{
    public ReportPage(ReportViewModel reportViewModel)
	{
		InitializeComponent();
        reportViewModel._navigation = this.Navigation;
        BindingContext = reportViewModel;
    }
}