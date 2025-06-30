using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StockApp.Application.DTOs;
using StockApp.Application.Interfaces;
using StockApp.Application.Services;
using StockApp.Domain.Interfaces;
using System.Text;

using StockApp.API.Hubs;
using StockApp.Domain.Entities;

namespace StockApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly IAuditService _auditService;
        private readonly IHubContext<StockHub> _hubContext;    
        public ProductsController(IProductService productService,IAuditService auditService, IHubContext<StockHub> hubContext, IInventoryService inventoryService)
        {
            _productService = productService;
            _auditService = auditService;
            _hubContext = hubContext;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtos = await _productService.GetProducts();
            if (produtos == null)
            {
                return NotFound("Products not found");
            }
            return Ok(produtos);
        }
        [HttpGet("{id}",Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var produto = await _productService.GetProductById(id);
            if (produto == null)
            {
               return NotFound("Product not found");
            }
            return Ok(produto);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
        {
           if (productDTO == null)
                 return BadRequest("Data Invalid");

              await _productService.Add(productDTO);

             return new CreatedAtRouteResult("GetProduct",
                 new {id = productDTO.Id }, productDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                 return BadRequest("Data invalid");
            }
            if (productDTO == null)
                 return BadRequest("Data invalid");
             await _productService.Update(productDTO);
             
            await _auditService.AuditStockChange(productDTO.Id, productDTO.Stock, productDTO.Stock, DateTime.UtcNow);
            return Ok(productDTO);
        }




        [HttpGet("lowstock" ,Name = "GetLowStockProducts")]
         public async Task<ActionResult<IEnumerable<ProductDTO>>> GetLowStockProducts(int limiteEstoque)
        {
             var produtoDTO = await _productService.EstoqueBaixo(limiteEstoque);
            if (produtoDTO == null)
             {
                return NotFound("Products not found");
             }
            
            return Ok(produtoDTO);
        }

        [HttpPut("bulk-update",Name ="BulkUpdateProducts")]
        public async Task<ActionResult> BulkUpdate([FromBody] List<ProductDTO> productsDTO)
        {
            if (productsDTO == null || !productsDTO.Any())
            {
                return BadRequest("No products to update");
            }
            
                await _productService.BulkUpdateAsync(productsDTO);
            return Ok(productsDTO);
        }
        [HttpPost("replenish")]
        public async Task<IActionResult> ReplenishStock()
        {
            await _inventoryService.ReplenishStockAsync();
            return Ok("Reposição Concluida!!!");
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToCsv()
        {
            var products = await _productService.GetProducts();
            if (products == null || !products.Any())
            {
                return NotFound("Products not found to export");
            }

            var csv = new StringBuilder();
            csv.AppendLine("Id,Name,Description,Price,Stock");

            foreach (var product in products)
            {
                csv.AppendLine($"{product.Id},{product.Name},{product.Description},{product.Price},{product.Stock}");
            }
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
        }
        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<Product>>> GetFiltered(
            [FromQuery] string name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var products = await _productService.GetFilteredAsync(name, minPrice, maxPrice);
            return Ok(products);
        }
    }
}
