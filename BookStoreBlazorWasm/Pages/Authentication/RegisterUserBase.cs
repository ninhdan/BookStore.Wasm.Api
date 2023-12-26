using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Pages.Book;
using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using BookStoreView.Models.Dtos.DtoUser;
using Microsoft.AspNetCore.Components;

namespace BookStoreBlazorWasm.Pages.Authentication
{
    public class RegisterUserBase : ComponentBase
    {
        [Inject] private IUserService _userService { get; set; }
        [Inject] private IToastService toastService { get; set; }

        [Inject] protected NavigationManager navigationManage { get; set; }

        [Inject] private IModalService Modal { get; set; } = default!;

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;

        protected RegisterUserDto newUser = new RegisterUserDto();

        public string ErrorMessage { get; set; }


        protected void GotoBackLogin()
        {
            navigationManage.NavigateTo("/login");
        }

        protected async Task RegisterUser()
        {
            try
            {
                var result = await _userService.RegisterUserAsync(newUser);


                if (result)
                {

                    string successMessage = _userService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    GotoBackLogin();
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



        protected void ShowSuccessMessage(string message)
        {

            toastService.ShowSuccess(message);
        }

        protected void ShowErrorMessage(string message)
        {
            toastService.ShowError(message);
        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }


        protected async Task Cancel()
        {
            if (BlazoredModalInstance != null)
            {
                await BlazoredModalInstance.CancelAsync();
            }
        }

    }
}
