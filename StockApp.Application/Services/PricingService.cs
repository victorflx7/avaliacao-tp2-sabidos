using StockApp.Domain.Interfaces;
using Newtonsoft.Json;
namespace StockApp.Application.Services
{
    public class PricingService : IPricingService
    {
        private readonly HttpClient _httpClient;
        public PricingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetProductPriceAsync(string productId)
        {
            var response = await _httpClient.GetAsync($"https://api.pricing.com/products/{productId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var price = JsonConvert.DeserializeObject<decimal>(content);

            return price;
        }
    }
}
