using LenayGG_Movil.Models;
using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Models.WalletModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Infrastructure
{
    public interface ITransactionInfraestructure
    {
        Task<ApiResponseDto> AddGasto(TransactionAggregate transaction, string token);
        Task<ApiResponseDto> AddIngreso(TransactionAggregate transactionAggregate, string token);
        Task<ApiResponseDto> AddTransferencia(TransferAggregate transferAggregate, string token);
        Task<object> GetTransaccionesByIdWallet(IdWalletDto idWalletDto, string token);
        Task<object> GetTransaccionesByIdUsuario(string token);
        Task<object> categoriaDtos(string token);
    }
}
