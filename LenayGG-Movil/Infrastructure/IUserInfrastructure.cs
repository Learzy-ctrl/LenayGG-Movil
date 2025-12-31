using LenayGG_Movil.Models;
using LenayGG_Movil.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.Infrastructure
{
    public interface IUserInfrastructure
    {
        Task<ApiResponseDto> UploadImageAsync(PhotoUserAggregate aggregate, string token);

        Task<ApiResponseDto> UpdateUserAsync(UpdateUserAggregate aggregate, string token);

        Task<UserDto> GetUserAsync(string token);

        Task<ApiResponseDto> DeleteUserAsync(string token);

        Task<ApiResponseDto> ChangePasswordAsync(PasswordAggregate aggregate, string token);
    }
}
