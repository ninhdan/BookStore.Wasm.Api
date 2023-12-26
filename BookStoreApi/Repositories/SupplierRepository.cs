
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;

namespace BookStoreApi.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DbWater7Context _context;

        public SupplierRepository(DbWater7Context context)
        {
            _context = context;
        }

        public bool CreateSupplier(Supplier Supplier)
        {
            _context.Add(Supplier);
            return Save();
        }

        public bool DeleteSupplier(Supplier Supplier)
        {
            _context.Remove(Supplier);
            return Save();
        }

        public ICollection<Supplier> GetAllSupplier()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier GetSupplier(Guid SupplierId)
        {
            return _context.Suppliers.Where(c => c.SupplierId == SupplierId).FirstOrDefault();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;

        }

        public bool SupplierExists(Guid SupplierId)
        {
            return _context.Suppliers.Any(c => c.SupplierId == SupplierId);
        }

        public bool SupplierNameExists(string SupplierName)
        {
            return _context.Suppliers.Any(c => c.SupplierName.ToLower().Trim() == SupplierName.ToLower().Trim());
        }

        public bool UpdateSupplier(Supplier Supplier)
        {
            _context.Update(Supplier);
            return Save();
        }
    }
}
