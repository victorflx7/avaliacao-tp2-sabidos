using StockApp.Domain.Entities;

namespace StockApp.Domain.Interfaces
{
    internal interface ISupplierRepository
    {
        Task<Supplier> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}
