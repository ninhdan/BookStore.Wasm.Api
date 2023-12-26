using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Pages.Category;
using BookStoreBlazorWasm.Pages.Layout;
using BookStoreBlazorWasm.Services;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;
using System.Linq;


namespace BookStoreBlazorWasm.Pages.SubCategory
{
    public class SubCategoryBase : ComponentBase
    {

        [Inject] private ISubCategoryService _subCategoryService { get; set; }
        [Inject] private IToastService toastService { get; set; }

        public IEnumerable<SubCategoryDto> _subCategories;
        [Inject] private IModalService Modal { get; set; } = default!;
        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
        public string ErrorMessage { get; set; }

        public SubCategoryDto newSubCategory = new SubCategoryDto();
        protected bool IsAnySubCategorySelected => _subCategories.Count(c => c.IsSelected) > 0;
        protected bool IsSingleSubCategorySelected => _subCategories.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedSubCategoryNames => _subCategories?.Where(c => c.IsSelected).Select(c => c.SubCategoryName).ToList() ?? new List<string>();

        [Parameter] public List<SubCategoryDto> SelectedSubCategories { get; set; } = new List<SubCategoryDto>();
        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var subcategory in _subCategories)
            {
                subcategory.IsSelected = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadSubCategories();
        }


        private async Task LoadSubCategories()
        {

            _subCategories = await _subCategoryService.GetAllSubCategory();
        }


        protected async Task ShowConfirmationAddModal()
        {
            var parameters = new ModalParameters();

            var modalInstance = Modal.Show<CreateSubCategory>("Add New SubCategory", parameters);

            var modalResult = await modalInstance.Result;

            if (!modalResult.Cancelled)
            {
                await AddNewSubCategory();

            }
            else
            {
                await LoadSubCategories();
            }

        }

        protected async Task AddNewSubCategory()
        {

            try
            {
               
                var result = await _subCategoryService.CreateSubCategory(newSubCategory);

                if (result)
                {
                    await RefreshUIAfterAddingSubCategory();
                    string successMessage = _subCategoryService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);

                }
                else
                {
                    string errorMessage = _subCategoryService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }

        }

        private async Task RefreshUIAfterAddingSubCategory()
        {
            await LoadSubCategories();
            ResetNewSubCategory();
        }
        private void ResetNewSubCategory()
        {
            newSubCategory = new SubCategoryDto();
        }

        private void ShowSuccessMessage(string message)
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

        private async Task RefreshUIAfterDeletingSubCategories(List<SubCategoryDto> deletedLayouts)
        {

            _subCategories = _subCategories.Except(deletedLayouts).ToList();
            string successMessage = _subCategoryService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadSubCategories();
        }
        protected async Task DeleteSelected()
        {
            var selectedSubCategory = _subCategories?.Where(c => c.IsSelected).ToList();

            if (selectedSubCategory != null && selectedSubCategory.Any())
            {
                SelectedSubCategories = selectedSubCategory;
                var parameters = new ModalParameters();
                parameters.Add("SelectedSubCategoryNames", SelectedSubCategoryNames);

                var modalInstance = Modal.Show<DeleteSubCategory>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingSubCategories(selectedSubCategory);
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
                if (_subCategoryService != null)
                {

                    foreach (var subcategory in SelectedSubCategories)
                    {
                        await _subCategoryService.DeleteSubCategory(subcategory.SubCategoryId);
                    }
                    await BlazoredModalInstance.CloseAsync();

                }
                else
                {
                    ShowErrorMessage("categoryService is null.");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred during deletion: {ex.Message}");

            }

        }

        [Parameter] public SubCategoryDto SubCategoryToEdit { get; set; } = new SubCategoryDto();
       


        // Edit category
        protected async Task ShowEditModal()
        {
            var selectedSubCategory = _subCategories.FirstOrDefault(c => c.IsSelected);
            if (IsSingleSubCategorySelected)
            {
                SubCategoryToEdit = selectedSubCategory; // Set the selected category to edit


                var parameters = new ModalParameters();
                parameters.Add("SubCategoryToEdit", SubCategoryToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditSubCategory>("Edit SubCategory", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await UpdateSubCategory();
                    //return;

                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdateSubCategory()
        {
            try
            {

                var result = await _subCategoryService.UpdateSubCategory(SubCategoryToEdit);
                if (result)
                {

                    string successMessage = _subCategoryService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    await RefreshUIAfterUpdatingSubCategory();

                }
                else
                {
                    string errorMessage = _subCategoryService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }

        private async Task RefreshUIAfterUpdatingSubCategory()
        {
            await LoadSubCategories();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }




    }
}
