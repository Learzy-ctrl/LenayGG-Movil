using LenayGG_Movil.Models;
using LenayGG_Movil.Models.LoginModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Infrastructure
{
    public interface ILogin
    {
        Task<ApiResponseDto> SignUp(string username, DateTime birthDate, string email, string password);

        Task<ApiResponseDto> SignIn(string _email, string _password);

        Task<ApiResponseDto> ResetPasswordByEmail(ResetPasswprdAggregate aggregate);
    }
}
