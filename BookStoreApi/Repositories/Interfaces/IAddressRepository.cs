using BookStoreApi.Models;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;

namespace BookStoreApi.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        ICollection<Address> GetAllAddress();
        Address GetAddress(Guid AddressId);
        bool AddressExists(Guid AddressId);
        bool UpdateAddress(Address Address);
        bool DeleteAddress(Address Address);
        bool Save();
        Address ConvertToEntity(AddressDto addressDto);
        AddressDto ConvertToDto(Address address);
        Task<AddressDto> AddAddress(AddressDto address);
        Task<int> UpdateOldAddresses(Guid userId);
        Task<List<Address>> GetListAddressByUserIdAsync(Guid userId);












    }
}
