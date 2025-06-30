using System.Threading.Tasks;

namespace StockApp.Application.Interfaces
{
    public interface IErpIntegrationService
    {
        Task SyncDataAsync();
    }
}
