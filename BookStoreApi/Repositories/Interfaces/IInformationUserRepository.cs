using BookStoreApi.Models;
using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IInformationUserRepository
    {
        Task<User> UpdateUser(UpdateUserDto userUpdateDto);

        Task<User> GetUserById(Guid id);
        bool UserExists(Guid userId);


    }
}
