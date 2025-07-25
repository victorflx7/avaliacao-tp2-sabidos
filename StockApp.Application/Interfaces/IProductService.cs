﻿using StockApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int? id);
        Task Add(ProductDTO productDto);
        Task Update(ProductDTO productDto);
        Task Remove(int? id);
        Task<IEnumerable<ProductDTO>>  EstoqueBaixo(int limiteEstoque);
        Task BulkUpdateAsync(List<ProductDTO> productsDTO);
        Task<IEnumerable<ProductDTO>> GetFilteredAsync(string name, decimal? minPrice, decimal? maxPrice);
    }
}
