namespace StockApp.Domain.Interfaces
{
    public interface IPricingService
    {
        Task<decimal> GetProductPriceAsync(string productId);
    }
}
