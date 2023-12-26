using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;

namespace BookStoreApi.Repositories
{
    public class PriceRangeRepository : IPriceRangeRepository
    {
        private readonly DbWater7Context _context;
      
        public PriceRangeRepository(DbWater7Context context)
        {
            _context = context;
           
        } 

        public bool CreatePriceRange(PriceRange priceRange)
        {
            _context.Add(priceRange);
            return Save();
        }

        public bool DeletePriceRange(PriceRange priceRange)
        {
            _context.Remove(priceRange);
            return Save();
        }

        public ICollection<PriceRange> GetAllPriceRange()
        {
            return _context.PriceRanges.ToList();
        }

        public PriceRange GetPriceRange(Guid priceRangeId)
        {
            return _context.PriceRanges.Where(c => c.RangeId == priceRangeId).FirstOrDefault();
        }

        public bool PriceRangeExists(Guid priceRangeId)
        {
            return _context.PriceRanges.Any(c => c.RangeId == priceRangeId);
        }

        public bool PriceRangeNameExists(string priceRangeName)
        {
            return _context.PriceRanges.Any(c => c.RangeName.ToLower().Trim() == priceRangeName.ToLower().Trim());
        }

        public bool Save()
        {
           return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdatePriceRange(PriceRange priceRange)
        {
            _context.Update(priceRange);
            return Save();
        }
    }
}
