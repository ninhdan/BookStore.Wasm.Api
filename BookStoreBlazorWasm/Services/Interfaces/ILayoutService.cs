using BookStoreView.Models.Dtos;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<IEnumerable<LayoutDto>> GetLayouts();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<LayoutDto> GetLayout(Guid layoutId);
        Task<bool> CreateLayout(LayoutDto layoutDto);
        Task<bool> UpdateLayout(LayoutDto layoutDto);
        Task<bool> DeleteLayout(Guid layoutId);

    }
}
