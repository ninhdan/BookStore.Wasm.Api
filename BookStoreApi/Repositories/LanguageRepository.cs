
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;

namespace BookStoreApi.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly DbWater7Context _context;

        public LanguageRepository(DbWater7Context context)
        {
            _context = context;
        }


        public bool CreateLanguage(Language language)
        {
            _context.Add(language);
            return Save();
        }

        public bool DeleteLanguage(Language language)
        {
            _context.Remove(language);
            return Save();
        }

        public Language GetLanguage(Guid languageId)
        {
           return _context.Languages.Where(l => l.LanguageId == languageId).FirstOrDefault();
        }

        public Language GetLanguage(string languageCode)
        {
            return _context.Languages.FirstOrDefault(lang => lang.LanguageCode == languageCode);
        }

        public Language GetLanguageByName(string languageName)
        {
            return _context.Languages.FirstOrDefault(lang => lang.LanguageName.Equals(languageName, StringComparison.OrdinalIgnoreCase));

        }

        public ICollection<Language> GetLanguages()
        {
            return _context.Languages.ToList();
        }

        public bool LanguageExists(Guid languageId)
        {
            return _context.Languages.Any(c => c.LanguageId == languageId);
        }

        public bool LanguageExists(string languageCode)
        {
            return _context.Languages.Any(c => c.LanguageCode.ToLower().Trim() == languageCode.ToLower().Trim());
        }

        public bool LanguageNameExists(string languageName)
        {
            return _context.Languages.Any(c => c.LanguageName.ToLower().Trim() == languageName.ToLower().Trim());
        }


        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateLanguage(Language language)
        {
            _context.Update(language);
            return Save();
        }
    }
}
