using BookStoreView.Models.Dtos;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageDto>> GetLanguages();
        public string GetErrorMessage();
        public string GetSuccessMessage();
        Task<LanguageDto> GetLanguage(Guid languageId);
        Task<bool> CreateLanguage(LanguageDto languageDto);
        Task<bool> UpdateLanguage(LanguageDto languageDto);
        Task<bool> DeleteLanguage(Guid languageId);

    }
}
