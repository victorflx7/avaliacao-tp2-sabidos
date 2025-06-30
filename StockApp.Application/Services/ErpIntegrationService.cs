using StockApp.Application.Interfaces;
using StockApp.Domain.Interfaces;
using StockApp.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace StockApp.Application.Services
{
    public class ErpIntegrationService : IErpIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly IProductRepository _productRepository;
        
        public ErpIntegrationService(HttpClient httpClient, IProductRepository productRepository) 
        {
        _httpClient = httpClient;
        _productRepository = productRepository;
        }

        public async Task SyncDataAsync()
        {
            var response = await _httpClient.GetAsync("https://erp.external.com/api/products");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var existing = await _productRepository.GetById(product.Id);
                    if (existing == null)
                    {
                        await _productRepository.Create(product);
                    }
                    else
                    {
                        await _productRepository.Update(product);
                    }
                }
            }
        }
    }
}
