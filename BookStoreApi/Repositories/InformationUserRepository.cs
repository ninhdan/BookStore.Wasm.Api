using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories
{
    public class InformationUserRepository : IInformationUserRepository
    {

        private readonly IMapper _mapper;
        private readonly DbWater7Context context;

        public InformationUserRepository(IMapper mapper, DbWater7Context context)
        {
            _mapper = mapper;
            this.context = context;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await context.Users.Where(c => c.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUser(UpdateUserDto userUpdateDto)
        {
            var user = await context.Users.FindAsync(userUpdateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            _mapper.Map(userUpdateDto, user);

            await context.SaveChangesAsync();

            return user;
        }

        public bool UserExists(Guid userId)
        {
            return context.Users.Any(c => c.UserId == userId);
        }

    }
}
