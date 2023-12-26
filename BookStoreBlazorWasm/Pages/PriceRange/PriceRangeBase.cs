using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace BookStoreBlazorWasm.Pages.PriceRange
{
    public class PriceRangeBase : ComponentBase
    {
        [Inject] private IPriceRangeService priceRangeService { get; set; }
        [Inject] private IToastService toastService { get; set; }



        protected IEnumerable<PriceRangeDto> _priceRanges;
        protected PriceRangeDto newPriceRange = new PriceRangeDto();
        protected string ErrorMessage { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
        [Inject] private IModalService Modal { get; set; } = default!;


        protected bool IsAnypriceRangeSelected => _priceRanges.Count(c => c.IsSelected) > 0;
        protected bool IsSinglepriceRangeSelected => _priceRanges.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedPriceRangeNames => _priceRanges?.Where(c => c.IsSelected).Select(c => c.PriceRangeName).ToList() ?? new List<string>();

        [Parameter] public List<PriceRangeDto> SelectedPriceRanges { get; set; } = new List<PriceRangeDto>();
        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var priceRanges in _priceRanges)
            {
                priceRanges.IsSelected = value;
            }
        }


        // Load categories
        protected override async Task OnInitializedAsync()
        {
            await LoadPriceRanges();
        }

        private async Task LoadPriceRanges()
        {

            _priceRanges = await priceRangeService.GetPriceRanges();
        }

        protected async Task AddNewPriceRange()
        {

            try
            {
                var result = await priceRangeService.CreatePriceRange(newPriceRange);

                if (result)
                {
                    await RefreshUIAfterAddingpriceRange();
                    string successMessage = priceRangeService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);

                }
                else
                {
                    string errorMessage = priceRangeService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }

        }


        private async Task RefreshUIAfterAddingpriceRange()
        {
            await LoadPriceRanges();
            ResetNewPiceRange();
        }

        private void ResetNewPiceRange()
        {
            newPriceRange = new PriceRangeDto();
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

            var modalInstance = Modal.Show<CreatePriceRange>("Add New Price Range", parameters);

            var modalResult = await modalInstance.Result;

            if (!modalResult.Cancelled)
            {
                await AddNewPriceRange();

            }
            else
            {
                await LoadPriceRanges();
            }

        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }


        private async Task RefreshUIAfterDeletingpriceRanges(List<PriceRangeDto> deletedPriceRanges)
        {

            _priceRanges = _priceRanges.Except(deletedPriceRanges).ToList();
            string successMessage = priceRangeService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadPriceRanges();
        }
        protected async Task DeleteSelected()
        {
            var selectedpriceRanges = _priceRanges?.Where(c => c.IsSelected).ToList();

            if (selectedpriceRanges != null && selectedpriceRanges.Any())
            {
                SelectedPriceRanges = selectedpriceRanges;
                var parameters = new ModalParameters();
                parameters.Add("SelectedpriceRangeNames", SelectedPriceRangeNames);

                var modalInstance = Modal.Show<DeletePriceRange>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingpriceRanges(selectedpriceRanges);
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
                if (priceRangeService != null)
                {

                    foreach (var priceRange in SelectedPriceRanges)
                    {
                        await priceRangeService.DeletePriceRange(priceRange.PriceRangeId);
                    }
                    await BlazoredModalInstance.CloseAsync();

                }
                else
                {
                    ShowErrorMessage("priceRangeService is null.");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred during deletion: {ex.Message}");

            }

        }

        //private EditContext editContext ;
        [Parameter] public PriceRangeDto priceRangeToEdit { get; set; } = new PriceRangeDto();
        protected bool ShowConfirmationForm { get; set; } = false;


        // Edit category
        protected async Task ShowEditModal()
        {
            var selectedPriceRange = _priceRanges.FirstOrDefault(c => c.IsSelected);
            if (IsSinglepriceRangeSelected)
            {
                priceRangeToEdit = selectedPriceRange; // Set the selected category to edit


                var parameters = new ModalParameters();
                parameters.Add("priceRangeToEdit", priceRangeToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditPriceRange>("Edit Price Range", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await UpdatePriceRange();


                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdatePriceRange()
        {
            try
            {

                var result = await priceRangeService.UpdatePriceRange(priceRangeToEdit);

                if (result)
                {

                    string successMessage = priceRangeService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    await RefreshUIAfterUpdatingpriceRange();

                }
                else
                {
                    string errorMessage = priceRangeService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }



        private async Task RefreshUIAfterUpdatingpriceRange()
        {
            await LoadPriceRanges();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }
    }
}
