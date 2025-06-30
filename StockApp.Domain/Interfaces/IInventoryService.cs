namespace StockApp.Domain.Interfaces
{
    public interface IInventoryService
    {
        Task ReplenishStockAsync();
    }
}
