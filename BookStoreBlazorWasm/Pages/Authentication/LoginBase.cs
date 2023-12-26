using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using System.Web;

namespace BookStoreBlazorWasm.Pages.Authentication
{
    public class LoginBase : ComponentBase
    {
        [Inject] private IUserService _userService { get; set; }
        [Inject] private IToastService toastService { get; set; }

        [Inject] protected NavigationManager navigationManage { get; set; }

        protected LoginUserDto loginUser = new LoginUserDto();

        public string ErrorMessage { get; set; }
        protected async Task LoginUser()
        {
            try
            {
                var result = await _userService.LoginUserAsync(loginUser);


                if (result)
                {
                    //var absoluteUri = new Uri(navigationManage.Uri, UriKind.RelativeOrAbsolute);
                    //var query = HttpUtility.ParseQueryString(absoluteUri.Query);
                    //var returnUrl = query.Get("returnUrl");

                    //navigationManage.NavigateTo(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
                    string successMessage = _userService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                  
                }
                else
                {
                    string errorMessage = _userService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }

        }


        protected void OnRegister()
        {
            navigationManage.NavigateTo("/register");
        }
        protected void OnResetPassword()
        {
            navigationManage.NavigateTo("/send-password");
        }


        protected void ShowSuccessMessage(string message)
        {

            toastService.ShowSuccess(message);
        }

        protected void ShowErrorMessage(string message)
        {
            toastService.ShowError(message);
        }








    }
}
