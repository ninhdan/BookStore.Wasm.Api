using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;

namespace BookStoreApi.Repositories
{
    public class LayoutRepository: ILayoutRepository
    {
        private readonly DbWater7Context _bookStoreContext;

        public LayoutRepository(DbWater7Context bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }

        public ICollection<Layout> GetAllLayout()
        {
            return _bookStoreContext.Layouts.ToList();
        }

        public Layout GetLayout(Guid LayoutId)
        {
            return _bookStoreContext.Layouts.FirstOrDefault(l => l.LayoutId == LayoutId);
        }

        public bool LayoutExists(Guid LayoutId)
        {
            return _bookStoreContext.Layouts.Any(l => l.LayoutId == LayoutId);
        }

        public bool LayoutNameExists(string LayoutName)
        {
            return _bookStoreContext.Layouts.Any(l => l.LayoutName.ToLower().Trim() == LayoutName.ToLower().Trim());
        }

        public bool CreateLayout(Layout Layout)
        {
            _bookStoreContext.Add(Layout);
            return Save();
        }

        public bool UpdateLayout(Layout Layout)
        {
            _bookStoreContext.Update(Layout);
            return Save();
        }

        public bool DeleteLayout(Layout Layout)
        {
            _bookStoreContext.Remove(Layout);
            return Save();
        }

        public bool Save()
        {
            var saved = _bookStoreContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
