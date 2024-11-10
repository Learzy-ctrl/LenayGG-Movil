using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.UserModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LenayGG_Movil.Services
{
    public class UserService : IUserInfrastructure
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> ChangePasswordAsync(PasswordAggregate aggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(aggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/User/ChangeUserPassword", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 3,
                    Resultado = ex.Message,
                };
            }
        }

        public async Task<ApiResponseDto> DeleteUserAsync(string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/User/DeleteUser", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 3,
                    Resultado = ex.Message,
                };
            }
        }

        public async Task<UserDto> GetUserAsync(string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/User/GetUser", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    return new UserDto
                    {
                        NombreUser = "Token expirado"
                    };
                }
                var result = JsonConvert.DeserializeObject<UserDto>(responseData);
                return result;
            }
            catch (Exception ex)
            {
                return new UserDto
                {
                    NombreUser = "Error de conexion"
                };
            }
        }

        public async Task<ApiResponseDto> UpdateUserAsync(UpdateUserAggregate aggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(aggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/User/UpdateUser", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 3,
                    Resultado = ex.Message,
                };
            }
        }

        public async Task<ApiResponseDto> UploadImageAsync(PhotoUserAggregate aggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(aggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/User/UploadImage", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 3,
                    Resultado = ex.Message,
                };
            }
        }
    }
}
