using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models.TransactionModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.WalletModel;

namespace LenayGG_Movil.Services
{
    public class TransactionService : ITransactionInfraestructure
    {
        private readonly HttpClient _httpClient;
        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponseDto> AddGasto(TransactionAggregate transaction, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/AddGasto", content);
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

        public async Task<ApiResponseDto> AddIngreso(TransactionAggregate transactionAggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(transactionAggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/AddIngreso", content);
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

        public async Task<ApiResponseDto> AddTransferencia(TransferAggregate transferAggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(transferAggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/AddTransferencia", content);
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

        public async Task<object> GetTransaccionesByIdWallet(IdWalletDto idWalletDto, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(idWalletDto), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/GetTransaccionesByIdWallet", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var result = JsonConvert.DeserializeObject<List<TransaccionDto>>(responseData);
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

        public async Task<object> GetTransaccionesByIdUsuario(string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/GetTransaccionesByIdUsuario", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var result = JsonConvert.DeserializeObject<List<TransaccionDto>>(responseData);
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

        public async Task<object> categoriaDtos(string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Transaction/GetCategorias", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var result = JsonConvert.DeserializeObject<List<CategoriaDto>>(responseData);
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
