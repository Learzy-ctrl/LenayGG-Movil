using LenayGG_Movil.Models.TransactionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Infrastructure
{
    public interface ITransactionInfraestructure
    {
        Task<object> categoriaDtos(string token);
    }
}
