using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IInformationUserService
    {
        Task<bool> UpdateUser(UpdateUserDto userUpdateDto);
        Task<UpdateUserDto> GetUserById(Guid userId);
    }
}
