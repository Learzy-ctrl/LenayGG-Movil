using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.LoginModel
{
    public class SignUpDto
    {
        public string Id {  get; set; }
        public string nombreUser { get; set; }
        public string contrasenia { get; set; }
        public string email { get; set; }
        public DateTime fechaNacimiento { get; set; }
    }
}
