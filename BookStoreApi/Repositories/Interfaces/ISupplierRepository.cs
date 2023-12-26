using BookStoreApi.Models;


namespace BookStoreApi.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        ICollection<Supplier> GetAllSupplier();
        Supplier GetSupplier(Guid SupplierId);
        bool SupplierExists(Guid SupplierId);
        bool SupplierNameExists(string SupplierName);
        bool CreateSupplier(Supplier Supplier);
        bool UpdateSupplier(Supplier Supplier);
        bool DeleteSupplier(Supplier Supplier);
        bool Save();
    }
}
