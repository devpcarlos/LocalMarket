using LocalMarket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalMarket.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetByBusinessIdAsync(Guid businessId);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
