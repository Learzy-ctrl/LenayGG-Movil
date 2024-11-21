using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Tools.Account;
using LenayGG_Movil.Views.Tools.Reports;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Tools
{
    public class ToolsViewModel
    {
        public INavigation _navigation { get; set; }
        private IServiceProvider _serviceProvider;
        public ToolsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            GoToAccountCommand = new AsyncRelayCommand(GoToAccount);
            GoToReportsCommand = new AsyncRelayCommand(GoToReports);
        }

        #region Methods
        private async Task GoToAccount()
        {
            var userPage = _serviceProvider.GetService<UserPage>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(userPage);
            UserDialogs.Instance.HideLoading();
        }

        private async Task GoToReports()
        {
            var reportPage = _serviceProvider.GetService<ReportPage>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(reportPage);
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Commands
        public ICommand GoToAccountCommand { get; private set; }
        public ICommand GoToReportsCommand { get; private set; }
        #endregion
    }
}
