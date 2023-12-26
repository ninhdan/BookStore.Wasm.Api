using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace BookStoreBlazorWasm.Pages.InformationUser
{
    public class AddressBase : ComponentBase
    {
        [Inject] IAddressService addressService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected AddressDto newAddress = new AddressDto { Country = "Việt Nam" , };

        protected UserInfoDto userInfo = new UserInfoDto();

        [Inject] private IToastService toastService { get; set; }



        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync(); 
            userInfo = await GetUserInfoAsync();
            StateHasChanged();
        }

        private async Task<UserInfoDto> GetUserInfoAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return new UserInfoDto
            {
                fistName = user.FindFirst("firstname")?.Value,
                lastName = user.FindFirst("lastname")?.Value,
                Phone = user.FindFirst("phone")?.Value
            };
        }




        private async Task<Guid?> GetUserIdAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return Guid.TryParse(user.FindFirst("sub")?.Value, out var userId) ? userId : (Guid?)null;
        }

        protected async Task CreateAddress()
        {

            var userId = await GetUserIdAsync();
            if(userId.HasValue) {
            
                newAddress.UserId = userId.Value;

                var result = await addressService.AddAddress(newAddress);
                if(result != null)
                {
                    ShowSuccessMessage("Thêm địa chỉ mới thành công");
                }
                else
                {
                    ShowErrorMessage("Thêm địa chỉ thất bại");
                }
            }
            else
            {
                ShowErrorMessage("Chưa đăng nhập tài khoản");
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
