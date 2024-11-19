using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.ReportModel
{
    public class ConsultaGastosAggregate
    {
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public string? id_usuario { get; set; }
        public Guid? id_billetera { get; set; }
        public bool todas_billeteras { get; set; }
    }
}
