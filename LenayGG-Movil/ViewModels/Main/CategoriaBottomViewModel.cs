using Acr.UserDialogs;
using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Views.Main.Transacciones;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Main
{
    public class CategoriaBottomViewModel : BaseViewModel
    {
        public CategoriaBottomSheet bottomSheet {  get; set; }
        private readonly ITransactionInfraestructure _transactionInfraestructure;
        private readonly ILogin _login;
        public ICommand SelectCategoriaCommand => new Command<CategoriaDto>(categoriaItem =>
        {
            MessagingCenter.Send(this, "CategoriaItemSelected", categoriaItem);
            bottomSheet.DismissAsync();
        });
        public CategoriaBottomViewModel(ITransactionInfraestructure transactionInfraestructure, ILogin login)
        {
            _transactionInfraestructure = transactionInfraestructure;
            _login = login;
            SetCategorias();
        }

        #region Variables
        private List<CategoriaDto> _categorias;
        #endregion

        #region Objects
        public List<CategoriaDto> Categorias
        {
            get { return _categorias; }
            set { SetValue(ref _categorias, value); }
        }
        #endregion

        #region Methods

        private async Task SetCategorias()
        {
            Categorias = await GetCategorias();
        }

        private async Task<List<CategoriaDto>> GetCategorias()
        {
            bool sen = false;
            List<CategoriaDto> list = null;
            do
            {
                var token = SecureStorage.GetAsync("Token").Result;
                var response = await _transactionInfraestructure.categoriaDtos(token);
                var apiResponse = response as ApiResponseDto;
                if (apiResponse != null)
                {
                    if (apiResponse.Resultado == "Connection failure")
                    {
                        var listCategoria = new List<CategoriaDto>();
                        var categoria = new CategoriaDto
                        {
                            Nombre = "Connection failure"
                        };
                        listCategoria.Add(categoria);
                        list = listCategoria;
                        sen = false;
                    }
                    else
                    {
                        var email = SecureStorage.GetAsync("Email").Result;
                        var password = SecureStorage.GetAsync("Password").Result;
                        var result = await _login.SignIn(email, password);
                        await SecureStorage.SetAsync("Token", result.Resultado);
                        sen = true;
                    }
                }
                else
                {
                    list = response as List<CategoriaDto>;
                    sen = false;
                }
            } while (sen);
            UserDialogs.Instance.HideLoading();
            return list;
        }
        #endregion


    }
}
