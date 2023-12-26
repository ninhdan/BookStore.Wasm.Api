using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreBlazorWasm.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDto> AddAddress(AddressDto address);
        Task<int> UpdateOldAddresses(Guid userId);
        Task<UserDto> GetUserByIdAsync(Guid userId);
    }
}
