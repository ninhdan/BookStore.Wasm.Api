using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DbWater7Context context;

        public AddressRepository(DbWater7Context context)
        {
            this.context = context;
        }


        public bool AddressExists(Guid AddressId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAddress(Address Address)
        {
            throw new NotImplementedException();
        }

        public Address GetAddress(Guid AddressId)
        {
            return context.Addresses.Where(c => c.AddressId == AddressId).FirstOrDefault();
        }

        public ICollection<Address> GetAllAddress()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateAddress(Address Address)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateOldAddresses(Guid userId)
        {
            var addresses = context.Addresses.Where(a => a.UserId == userId);
            foreach (var address in addresses)
            {
                address.StatusAddress = false;
            }


            return await context.SaveChangesAsync();
        }

        public async Task<AddressDto> AddAddress(AddressDto address)
        {

            var updatedAddressesCount = await UpdateOldAddresses(address.UserId);

            var addressEntity = ConvertToEntity(address);
            addressEntity.StatusAddress = true;

            context.Addresses.Add(addressEntity);
            await context.SaveChangesAsync();

            return ConvertToDto(addressEntity);
        }


        public async Task<List<Address>> GetListAddressByUserIdAsync(Guid userId)
        {
            var user = await context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.UserId == userId);
            return user?.Addresses.ToList();
        }



        public  Address ConvertToEntity(AddressDto addressDto)
        {
            return new Address
            {
                AddressId = addressDto.AddressId,
                StreetNumber = addressDto.StreetNumber,
                Ward = addressDto.Ward,
                City = addressDto.City,
                Province = addressDto.Province,
                Country = addressDto.Country,
                UserId = addressDto.UserId,
                StatusAddress = addressDto.StatusAddress
            };
        }

        public  AddressDto ConvertToDto(Address address)
        {
            return new AddressDto
            {
                AddressId = address.AddressId,
                StreetNumber = address.StreetNumber,
                Ward = address.Ward,
                City = address.City,
                Province = address.Province,
                Country = address.Country,
                UserId = address.UserId,
                StatusAddress = address.StatusAddress
            };
        }

    }
}
