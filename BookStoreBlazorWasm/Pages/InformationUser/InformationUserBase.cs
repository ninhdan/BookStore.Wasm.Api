using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreBlazorWasm.Pages.InformationUser
{
    public class InformationUserBase : ComponentBase
    {
        [Inject] IInformationUserService informationUserService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected UpdateUserDto UpdateUserDto = new UpdateUserDto();

        [Inject] private IToastService toastService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await LoadUserInfoAsync();
        }

        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?)null;
        }

        private async Task LoadUserInfoAsync()
        {
            var userId = await GetUserIdAsync();
            if (userId.HasValue)
            {
                UpdateUserDto = await informationUserService.GetUserById(userId.Value);
            }
            else
            {
                navigationManager.NavigateTo("/error");
            }
        }


        protected async Task HandleValidSubmit()
        {

            var success = await informationUserService.UpdateUser(UpdateUserDto);
            if (success)
            {
                ShowSuccessMessage("Cập nhật thông tin thành công!");
            }
            else
            {
                ShowErrorMessage("Cập nhật thông tin thất bại !");
            }



        }

        private void ShowSuccessMessage(string message)
        {

            toastService.ShowSuccess(message);
        }

        protected void ShowErrorMessage(string message)
        {
            toastService.ShowError(message);
        }




    }
}
