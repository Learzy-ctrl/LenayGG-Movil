using LenayGG_Movil.Models.ReportModel;

namespace LenayGG_Movil.Infrastructure
{
    public interface IReportInfrastructure
    {
        Task<object> GetReports(ConsultaGastosAggregate aggregate, string token);
    }
}
