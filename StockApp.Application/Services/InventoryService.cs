using StockApp.Application.Interfaces;
using StockApp.Domain.Interfaces;


namespace StockApp.Application.Services
{
    public class InventoryService : IInventoryService 
    {
        private readonly IProductService _productService;
        public InventoryService(IProductRepository productRepository)
        {
            _productService = _productService;
        }

        public async Task ReplenishStockAsync()
        {
            var lowStockProducts = await _productService.EstoqueBaixo(10);
            foreach (var product in lowStockProducts)
            {
                product.Stock += 50;
                await _productService.BulkUpdateAsync(product);
            }
        }
    }
}
