namespace StockApp.API.GraphQL;

public class Mutation
{
    public async Task<Product> AddProduct(ProductInput input, [Service] IProductRepository productRepository)
    {
        var product = new Product(input.Name, input.Description, input.Price, input.Stock, input.Image);
        return await productRepository.Create(product);
    }
}

public record ProductInput(string Name, string Description, decimal Price, int Stock, string Image);