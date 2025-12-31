using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LenayGG_Movil.Services
{
    public class WalletService : IWalletInfraestructure
    {

        private readonly HttpClient _httpClient;
        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ApiResponseDto> AddWallet(WalletAgregate walletDto, string token)
        {
            var content = new StringContent(JsonConvert.SerializeObject(walletDto), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsync("api/Wallet/AddWallet", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
            return result;
        }

        public async Task<ApiResponseDto> DeleteWallet(IdWalletDto idWallet, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(idWallet), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Wallet/DeleteWallet", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 2,
                    Resultado = ex.Message,
                };
            }

        }

        public async Task<object> GetWalletById(IdWalletDto idWallet, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(idWallet), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Wallet/GetWalletById", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var result = JsonConvert.DeserializeObject<WalletDto>(responseData);
                return result;
            }catch(Exception ex)
            {
                return new WalletDto
                {
                    Nombre = ex.Message
                };
            }
        }

        public async Task<object> GetWallets(string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Wallet/GetWallets", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var resultList = JsonConvert.DeserializeObject<List<WalletDto>>(responseData);
                return resultList;
            }catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 1,
                    Resultado = ex.Message
                };
            }
            
        }

        public async Task<ApiResponseDto> UpdateWallet(WalletAgregate walletDto, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(walletDto), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Wallet/UpdateWallet", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                return result;
            }catch(Exception ex)
            {
                return new ApiResponseDto
                {
                    NumError = 2,
                    Resultado = ex.Message
                };
            }
        }
    }
}
