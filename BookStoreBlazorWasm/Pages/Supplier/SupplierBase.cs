using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;
using BookStoreBlazorWasm.Services.Interfaces;

namespace BookStoreBlazorWasm.Pages.Supplier
{
    public class SupplierBase : ComponentBase
    {
        [Inject] private ISupplierService supplierService { get; set; }
        [Inject] private IToastService toastService { get; set; }



        protected IEnumerable<SupplierDto> _suppliers;
        protected SupplierDto newSupplier = new SupplierDto();
        protected string ErrorMessage { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
        [Inject] private IModalService Modal { get; set; } = default!;


        protected bool IsAnySupplierSelected => _suppliers.Count(c => c.IsSelected) > 0;
        protected bool IsSingleSupplierSelected => _suppliers.Count(c => c.IsSelected) == 1;

        protected List<string> SelectedSupplierNames => _suppliers?.Where(c => c.IsSelected).Select(c => c.SupplierName).ToList() ?? new List<string>();

        [Parameter] public List<SupplierDto> SelectedSuppliers { get; set; } = new List<SupplierDto>();
        protected bool selectAll;

        protected void SelectAll(bool value)
        {
            selectAll = value;
            foreach (var suppliers in _suppliers)
            {
                suppliers.IsSelected = value;
            }
        }


 
        protected override async Task OnInitializedAsync()
        {
            await LoadSuppliers();
        }

        private async Task LoadSuppliers()
        {

         
          _suppliers = await supplierService.GetAllSupplier();
      

        }

        protected async Task AddNewSupplier()
        {

            try
            {
                var result = await supplierService.CreateSupplier(newSupplier);

                if (result)
                {
                    await RefreshUIAfterAddingSupplier();
                    string successMessage = supplierService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);

                }
                else
                {
                    string errorMessage = supplierService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }

        }


        private async Task RefreshUIAfterAddingSupplier()
        {
            await LoadSuppliers();
            ResetNewSupplier();
        }

        private void ResetNewSupplier()
        {
            newSupplier = new SupplierDto();
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

            var modalInstance = Modal.Show<CreateSupplier>("Add New Supplier", parameters);

            var modalResult = await modalInstance.Result;

            if (!modalResult.Cancelled)
            {
                await AddNewSupplier();

            }
            else
            {
                await LoadSuppliers();
            }

        }

        protected async Task CloseModal()
        {
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }


        private async Task RefreshUIAfterDeletingSuppliers(List<SupplierDto> deletedSuppliers)
        {

            _suppliers = _suppliers.Except(deletedSuppliers).ToList();
            string successMessage = supplierService.GetSuccessMessage();
            ShowSuccessMessage(successMessage);
            await LoadSuppliers();
        }
        protected async Task DeleteSelected()
        {
            var selectedSuppliers = _suppliers?.Where(c => c.IsSelected).ToList();

            if (selectedSuppliers != null && selectedSuppliers.Any())
            {
                SelectedSuppliers = selectedSuppliers;
                var parameters = new ModalParameters();
                parameters.Add("SelectedSupplierNames", SelectedSupplierNames);

                var modalInstance = Modal.Show<DeleteSupplier>("Delete Confirmation", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await Delete();
                    await RefreshUIAfterDeletingSuppliers(selectedSuppliers);
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
                if (supplierService != null)
                {

                    foreach (var supplier in SelectedSuppliers)
                    {
                        await supplierService.DeleteSupplier(supplier.SupplierId);
                    }
                    await BlazoredModalInstance.CloseAsync();

                }
                else
                {
                    ShowErrorMessage("supplierService is null.");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred during deletion: {ex.Message}");

            }

        }

        //private EditContext editContext ;
        [Parameter] public SupplierDto SupplierToEdit { get; set; } = new SupplierDto();
        protected bool ShowConfirmationForm { get; set; } = false;


        // Edit category
        protected async Task ShowEditModal()
        {
            var selectedSupplier = _suppliers.FirstOrDefault(c => c.IsSelected);
            if (IsSingleSupplierSelected)
            {
                SupplierToEdit = selectedSupplier; // Set the selected category to edit


                var parameters = new ModalParameters();
                parameters.Add("SupplierToEdit", SupplierToEdit); // Pass the category to edit to the modal

                var modalInstance = Modal.Show<EditSupplier>("Edit Supplier", parameters);
                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await UpdateSupplier();
                    //return;

                }
            }
            else
            {
                ShowErrorMessage("Please select a single category for editing.");
            }
        }

        protected async Task UpdateSupplier()
        {
            try
            {

                var result = await supplierService.UpdateSupplier(SupplierToEdit);

                if (result)
                {

                    string successMessage = supplierService.GetSuccessMessage();
                    ShowSuccessMessage(successMessage);
                    await RefreshUIAfterUpdatingSupplier();

                }
                else
                {
                    string errorMessage = supplierService.GetErrorMessage();
                    ShowErrorMessage(errorMessage);
                }

            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }



        private async Task RefreshUIAfterUpdatingSupplier()
        {
            await LoadSuppliers();
            await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
        }
    }
}
