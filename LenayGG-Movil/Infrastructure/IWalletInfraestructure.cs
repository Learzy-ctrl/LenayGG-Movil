
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;

namespace LenayGG_Movil.Infrastructure
{
    public interface IWalletInfraestructure
    {
        Task<ApiResponseDto> AddWallet(WalletAgregate walletDto, string token);

        Task<object> GetWallets(string token);

        Task<object> GetWalletById(IdWalletDto idWallet, string token);

        Task<ApiResponseDto> UpdateWallet(WalletAgregate walletDto, string token);

        Task<ApiResponseDto> DeleteWallet(IdWalletDto idWallet, string token);
    }
}
