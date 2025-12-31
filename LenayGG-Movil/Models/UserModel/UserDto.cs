using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Models.UserModel
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? FotoUser { get; set; }
        public string NombreUser { get; set; }
        public string? ApellidoUsuario { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
