using BookStoreView.Models.Dtos;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IPriceRangeService
    {
        Task<IEnumerable<PriceRangeDto>> GetPriceRanges();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<PriceRangeDto> GetPriceRange(Guid priceRangeId);
        Task<bool> CreatePriceRange(PriceRangeDto priceRangeDto);
        Task<bool> UpdatePriceRange(PriceRangeDto priceRangeDto);
        Task<bool> DeletePriceRange(Guid priceRangeId);



    }
}
