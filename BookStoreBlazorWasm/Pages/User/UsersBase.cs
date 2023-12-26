using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Components;

namespace BookStoreBlazorWasm.Pages.User
{
    public class UsersBase : ComponentBase
    {
        [Inject] private IUserService _userService { get; set; }

        protected IEnumerable<UserDto> _user;

        protected string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            _user = await _userService.GetUsers();
        }


    }
}
