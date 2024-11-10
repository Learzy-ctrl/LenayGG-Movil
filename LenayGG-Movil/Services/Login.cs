using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.LoginModel;
using Newtonsoft.Json;
using System.Text;

namespace LenayGG_Movil.Services
{
    public class Login : ILogin
    {
        private readonly HttpClient _httpClient;
        public Login(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> SignUp(string username, DateTime birthDate, string email, string password)
        {
            var signUpDto = new SignUpDto
            {
                Id = "",
                nombreUser = username,
                email = email,
                contrasenia = password,
                fechaNacimiento = birthDate
            };
            var content = new StringContent(JsonConvert.SerializeObject(signUpDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Login/SignUp", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
            return result;
        }

        public async Task<ApiResponseDto> SignIn(string _email, string _password)
        {
            var signInDto = new SignInDto
            {
                password = _password,
                email = _email
            };
            var content = new StringContent(JsonConvert.SerializeObject(signInDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Login/SignIn", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
            return result;
        }

        public async Task<ApiResponseDto> ResetPasswordByEmail(ResetPasswprdAggregate aggregate)
        {
            var content = new StringContent(JsonConvert.SerializeObject(aggregate), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Login/ResetPassword", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
            return result;
        }
    }
}
