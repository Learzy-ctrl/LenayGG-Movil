using LenayGG_Movil.Infrastructure;
using LenayGG_Movil.Models;
using LenayGG_Movil.Models.ReportModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LenayGG_Movil.Services
{
    public class ReportService : IReportInfrastructure
    {
        private readonly HttpClient _httpClient;
        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> GetReports(ConsultaGastosAggregate aggregate, string token)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(aggregate), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync("api/Report/GetGastosByFilter", content);
                var responseData = await response.Content.ReadAsStringAsync();
                if (responseData == "{\"numError\":1,\"resultado\":\"Token Expirado o no valido\"}")
                {
                    var resultApi = JsonConvert.DeserializeObject<ApiResponseDto>(responseData);
                    return resultApi;
                }
                var result = JsonConvert.DeserializeObject<List<GastosPorCategoriaDto>>(responseData);
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
