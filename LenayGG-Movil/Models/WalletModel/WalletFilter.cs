using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.WalletModel
{
    public class WalletFilter
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public decimal Saldo { get; set; }
        public int IdTipoCuenta {  get; set; }
        public decimal LimiteCredito { get; set; }
        public string FechaDePago { get; set; }
        public string ColorWallet { get; set; }
        public string Label { get; set; }
        public bool isVisible { get; set; }

    }
}
