using BookStoreApi.Models;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface ILanguageRepository
    {
        ICollection<Language> GetLanguages();
        Language GetLanguage(Guid languageId);
        Language GetLanguage(string languageCode);
        bool LanguageExists(Guid languageId);
        bool LanguageExists(string languageCode);
        bool LanguageNameExists(string languageName);
        bool CreateLanguage(Language language);
        bool UpdateLanguage(Language language);
        bool DeleteLanguage(Language language);
        bool Save();
    }
}
