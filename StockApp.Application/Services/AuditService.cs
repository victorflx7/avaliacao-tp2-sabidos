using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockApp.Domain.Interfaces;

namespace StockApp.Application.Services
{
    public interface IAuditService
    {
       Task AuditStockChange(int productId, int oldStock, int newStock, DateTime changeDate);
    }
    public class AuditService : IAuditService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(IProductRepository productRepository,ILogger<AuditService> logger) 
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task AuditStockChange(int productId, int oldStock, int newStock, DateTime changeDate)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {productId} not found for stock change audit.");
                return;
            }
            var auditMessage = $"Product ID: {productId}, Name: {product.Name}, Old Stock: {oldStock}, New Stock: {newStock}, Change Date: {changeDate}";
            _logger.LogInformation(auditMessage);
        }
    }
}
