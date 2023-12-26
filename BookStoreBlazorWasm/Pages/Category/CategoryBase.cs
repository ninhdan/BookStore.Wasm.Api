using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BookStoreBlazorWasm.Services.Interfaces;


namespace BookStoreBlazorWasm.Pages.Category
{
    public class CategoryBase : ComponentBase
    {

        [Inject] private ICategoryService categoryService { get; set; }
        [Inject] private IToastService toastService { get; set; }

      

        protected IEnumerable<CategoryDto> _categories ;
        protected CategoryDto newCategory = new CategoryDto();
        protected string ErrorMessage { get; set; } 

        protected string bookVietNam = "Sách tiếng Việt";
        protected string bookForeign = "Foreign books";

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
        [Inject] private IModalService Modal { get; set; } = default!;


        protected bool IsAnyCategorySelected => _categories.Count(c => c.IsSelected) > 0;
        protected bool IsSingleCategorySelected => _categories.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedCategoryNames => _categories?.Where(c => c.IsSelected).Select(c => c.CategoryName).ToList() ?? new List<string>();

        [Parameter] public List<CategoryDto> SelectedCategories { get; set; } = new List<CategoryDto>();
        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var category in _categories)
            {
                category.IsSelected = value;
            }
        }


        // Load categories
        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {

            _categories = await categoryService.GetAllCategory();

        }

        protected async Task AddNewCategory()
        {
         
                try
                {
                    var result = await categoryService.CreateCategory(newCategory);

                    if (result)
                    {
                        await RefreshUIAfterAddingCategory();
                        string successMessage = categoryService.GetSuccessMessage();
                        ShowSuccessMessage(successMessage);
                      
                    }
                    else
                    {
                        string errorMessage = categoryService.GetErrorMessage();
                        ShowErrorMessage(errorMessage);
                    }
                   
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"An error occurred: {ex.Message}");
                }
            
        }


        private async Task RefreshUIAfterAddingCategory()
        {
            await LoadCategories();
            ResetNewCategory();
        }

        private void ResetNewCategory()
        {
            newCategory = new CategoryDto();
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

            var modalInstance = Modal.Show<CreateCategotyModal>("Add New Category", parameters);

            var modalResult = await modalInstance.Result;

            if (!modalResult.Cancelled)
            {
                await AddNewCategory();
               
            }else
            {
                await LoadCategories();
            }
           
        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }

     
        private async Task RefreshUIAfterDeletingCategories(List<CategoryDto> deletedCategories)
        {
           
            _categories = _categories.Except(deletedCategories).ToList();
            string successMessage = categoryService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadCategories();
        }
        protected async Task DeleteSelected()
        {
            var selectedCategories = _categories?.Where(c => c.IsSelected).ToList();

            if (selectedCategories != null && selectedCategories.Any())
            {
                SelectedCategories = selectedCategories;
                var parameters = new ModalParameters();
                parameters.Add("SelectedCategoryNames", SelectedCategoryNames);

                var modalInstance = Modal.Show<DeleteCategory>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingCategories(selectedCategories);
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
                if (categoryService != null)
                {
               
                    foreach (var category in SelectedCategories)
                    {
                        await categoryService.DeleteCategory(category.CategoryId);
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

        //private EditContext editContext ;
        [Parameter] public CategoryDto CategoryToEdit { get; set; } = new CategoryDto();
        protected bool ShowConfirmationForm { get; set; } = false;
       

        // Edit category
        protected async Task ShowEditModal()
        {
            var selectedCategory = _categories.FirstOrDefault(c => c.IsSelected);
            if (IsSingleCategorySelected)
            {
                CategoryToEdit = selectedCategory; // Set the selected category to edit
               

                var parameters = new ModalParameters();
                parameters.Add("CategoryToEdit", CategoryToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditCategory>("Edit Category", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                     await UpdateCategory();
                    //return;
                    
                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdateCategory()
        {
            try
            {

                    var result = await categoryService.UpdateCategory(CategoryToEdit);

                    if (result)
                    {
                        
                        string successMessage = categoryService.GetSuccessMessage();
                        ShowSuccessMessage(successMessage);
                        await RefreshUIAfterUpdatingCategory();
                        
                    }
                    else
                    {
                        string errorMessage = categoryService.GetErrorMessage();
                        ShowErrorMessage(errorMessage);
                    }
                
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                
            }
           
        }

       

        private async Task RefreshUIAfterUpdatingCategory()
        {  
            await LoadCategories();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }


    }
}

