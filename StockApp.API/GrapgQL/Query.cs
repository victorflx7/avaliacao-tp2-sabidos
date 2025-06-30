using HotChocolate;
using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;

namespace StockApp.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<Product>> GetProducts([Service] IProductRepository productRepository)
        => await productRepository.GetProducts();
}
