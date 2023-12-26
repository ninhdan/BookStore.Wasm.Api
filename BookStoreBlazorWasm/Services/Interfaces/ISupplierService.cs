using Blazored.Toast.Services;
using BookStoreView.Models.Dtos;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSupplier();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<SupplierDto> GetSupplier(Guid supplierId);
        Task<bool> CreateSupplier(SupplierDto supplierDto);
        Task<bool> UpdateSupplier(SupplierDto supplierDto);
        Task<bool> DeleteSupplier(Guid supplierId);
    }
}
