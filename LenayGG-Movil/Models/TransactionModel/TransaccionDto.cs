using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.TransactionModel
{
    public class TransaccionDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string CategoriaIcono { get; set; }
        public string CategoriaColor { get; set; }
        public string CategoriaNombre { get; set; }
        public decimal Dinero { get; set; }
        public string TipoTransaccion { get; set; }
    }
}
