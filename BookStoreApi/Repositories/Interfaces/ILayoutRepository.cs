using BookStoreApi.Models;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface ILayoutRepository
    {
        ICollection<Layout> GetAllLayout();
        Layout GetLayout(Guid LayoutId);
        bool LayoutExists(Guid LayoutId);
        bool LayoutNameExists(string LayoutName);
        bool CreateLayout(Layout category);
        bool UpdateLayout(Layout category);
        bool DeleteLayout(Layout category);
        bool Save();


    }
}
