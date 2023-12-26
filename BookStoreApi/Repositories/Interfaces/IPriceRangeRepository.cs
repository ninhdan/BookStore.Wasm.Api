using BookStoreApi.Models;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IPriceRangeRepository
    {
        ICollection<PriceRange> GetAllPriceRange();
        PriceRange GetPriceRange(Guid priceRangeId);
        bool PriceRangeExists(Guid priceRangeId);
        bool PriceRangeNameExists(string priceRangeName);
        bool CreatePriceRange(PriceRange priceRange);
        bool UpdatePriceRange(PriceRange priceRange);
        bool DeletePriceRange(PriceRange priceRange);
        bool Save();

    }
}
