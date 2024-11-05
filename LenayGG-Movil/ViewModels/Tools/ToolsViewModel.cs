using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Input;
using LenayGG_Movil.Views.Tools.Account;
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
        }

        #region Methods
        private async Task GoToAccount()
        {
            var userPage = _serviceProvider.GetService<UserPage>();
            UserDialogs.Instance.ShowLoading("Cargando");
            await _navigation.PushModalAsync(userPage);
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Commands
        public ICommand GoToAccountCommand { get; private set; }
        #endregion
    }
}
