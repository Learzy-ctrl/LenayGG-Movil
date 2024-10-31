using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.TransactionModel
{
    public class TransferAggregate
    {
        public TransactionAggregate Ingreso { get; set; }
        public TransactionAggregate Gasto { get; set; }
    }
}
