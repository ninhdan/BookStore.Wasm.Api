using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Pages.Language;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace BookStoreBlazorWasm.Pages.Language
{
    public class PriceRangeBase : ComponentBase
    {
        [Inject] private ILanguageService LanguageService { get; set; }
        [Inject] private IToastService toastService { get; set; }



        protected IEnumerable<LanguageDto> _Languages;
        protected LanguageDto newLanguage = new LanguageDto();
        protected string ErrorMessage { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
        [Inject] private IModalService Modal { get; set; } = default!;


        protected bool IsAnyLanguageSelected => _Languages.Count(c => c.IsSelected) > 0;
        protected bool IsSingleLanguageSelected => _Languages.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedLanguageNames => _Languages?.Where(c => c.IsSelected).Select(c => c.LanguageName).ToList() ?? new List<string>();

        [Parameter] public List<LanguageDto> SelectedLanguages { get; set; } = new List<LanguageDto>();
        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var Languages in _Languages)
            {
                Languages.IsSelected = value;
            }
        }


        // Load categories
        protected override async Task OnInitializedAsync()
        {
            await LoadLanguages();
        }

        private async Task LoadLanguages()
        {
  
            _Languages = await LanguageService.GetLanguages();
        }

        protected async Task AddNewLanguage()
        {

            try
            {
                var result = await LanguageService.CreateLanguage(newLanguage);

                if (result)
                {
                    await RefreshUIAfterAddingLanguage();
                    string successMessage = LanguageService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);

                }
                else
                {
                    string errorMessage = LanguageService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }

        }


        private async Task RefreshUIAfterAddingLanguage()
        {
            await LoadLanguages();
            ResetNewLanguage();
        }

        private void ResetNewLanguage()
        {
            newLanguage = new LanguageDto();
        }

        private void ShowSuccessMessage(string message)
        {

            toastService.ShowSuccess(message);
        }

        protected void ShowErrorMessage(string message)
        {
            toastService.ShowError(message);
        }


        protected async Task ShowConfirmationAddModal()
        {
            var parameters = new ModalParameters();

            var modalInstance = Modal.Show<CreateLanguage>("Add New Language", parameters);

            var modalResult = await modalInstance.Result;

            if (!modalResult.Cancelled)
            {
                await AddNewLanguage();

            }
            else
            {
                await LoadLanguages();
            }

        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }


        private async Task RefreshUIAfterDeletingLanguages(List<LanguageDto> deletedLanguages)
        {

            _Languages = _Languages.Except(deletedLanguages).ToList();
            string successMessage = LanguageService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadLanguages();
        }
        protected async Task DeleteSelected()
        {
            var selectedLanguages = _Languages?.Where(c => c.IsSelected).ToList();

            if (selectedLanguages != null && selectedLanguages.Any())
            {
                SelectedLanguages = selectedLanguages;
                var parameters = new ModalParameters();
                parameters.Add("SelectedLanguageNames", SelectedLanguageNames);

                var modalInstance = Modal.Show<DeleteLanguage>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingLanguages(selectedLanguages);
                }
            }
            else
            {
                ShowErrorMessage("No categories selected for deletion.");
            }
        }


        protected async Task Cancel()
        {
            if (BlazoredModalInstance != null)
            {
                await BlazoredModalInstance.CancelAsync();
            }

        }


        protected async Task Delete()
        {
            try
            {
                if (LanguageService != null)
                {

                    foreach (var Language in SelectedLanguages)
                    {
                        await LanguageService.DeleteLanguage(Language.LanguageId);
                    }
                    await BlazoredModalInstance.CloseAsync();

                }
                else
                {
                    ShowErrorMessage("LanguageService is null.");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred during deletion: {ex.Message}");

            }

        }

        //private EditContext editContext ;
        [Parameter] public LanguageDto LanguageToEdit { get; set; } = new LanguageDto();
        protected bool ShowConfirmationForm { get; set; } = false;


        // Edit category
        protected async Task ShowEditModal()
        {
            var selectedLanguage = _Languages.FirstOrDefault(c => c.IsSelected);
            if (IsSingleLanguageSelected)
            {
                LanguageToEdit = selectedLanguage; // Set the selected category to edit


                var parameters = new ModalParameters();
                parameters.Add("LanguageToEdit", LanguageToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditLanguage>("Edit Language", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await UpdateLanguage();
                 

                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdateLanguage()
        {
            try
            {

                var result = await LanguageService.UpdateLanguage(LanguageToEdit);

                if (result)
                {

                    string successMessage = LanguageService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    await RefreshUIAfterUpdatingLanguage();

                }
                else
                {
                    string errorMessage = LanguageService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }



        private async Task RefreshUIAfterUpdatingLanguage()
        {
            await LoadLanguages();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }
    }
}
