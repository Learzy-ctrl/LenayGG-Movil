using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.TransactionModel
{
    public class TransactionAggregate
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCategoria { get; set; }
        public Guid IdBilletera { get; set; }
        public decimal Dinero { get; set; }
        public string IdUsuario { get; set; }
    }
}
