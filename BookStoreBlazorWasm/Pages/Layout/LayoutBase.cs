using Blazored.Modal.Services;
using Blazored.Modal;
using Blazored.Toast.Services;
using BookStoreBlazorWasm.Pages.Layout;
using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace BookStoreBlazorWasm.Pages.Layout
{
    public class LayoutBase : ComponentBase
    {
     
            [Inject] private ILayoutService layoutService { get; set; }
            [Inject] private IToastService toastService { get; set; }


            protected IEnumerable<LayoutDto> _Layouts;

            protected LayoutDto newLayout = new LayoutDto();
            protected string ErrorMessage { get; set; }

            [CascadingParameter] BlazoredModalInstance BlazoredModalInstance { get; set; } = default!;
            [Inject] private IModalService Modal { get; set; } = default!;


            protected bool IsAnyLayoutSelected => _Layouts.Count(c => c.IsSelected) > 0;
            protected bool IsSingleLayoutSelected => _Layouts.Count(c => c.IsSelected) == 1;

             protected List<string> SelectedLayoutNames => _Layouts?.Where(c => c.IsSelected).Select(c => c.LayoutName).ToList()?? new List<string>();

            [Parameter] public List<LayoutDto> SelectedLayouts { get; set; } = new List<LayoutDto>();
            protected bool selectAll;

            protected void SelectAll(bool value)
            {
                selectAll = value;
                foreach (var Layout in _Layouts)
                {
                    Layout.IsSelected = value;
                }
            }


            // Load categories
            protected override async Task OnInitializedAsync()
            {
                await LoadLayouts();
            }

            private async Task LoadLayouts()
            {

                _Layouts = await layoutService.GetLayouts();

            }

            protected async Task AddNewLayout()
            {

                try
                {
                    var result = await layoutService.CreateLayout(newLayout);

                    if (result)
                    {
                        await RefreshUIAfterAddingLayout();
                        string successMessage = layoutService.GetSuccessMessage();
                        ShowSuccessMessage(successMessage);

                    }
                    else
                    {
                        string errorMessage = layoutService.GetErrorMessage();
                        ShowErrorMessage(errorMessage);
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"An error occurred: {ex.Message}");
                }

            }


            private async Task RefreshUIAfterAddingLayout()
            {
                await LoadLayouts();
                ResetNewLayouts();
            }

            private void ResetNewLayouts()
            {
                newLayout = new LayoutDto();
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

                var modalInstance = Modal.Show<CreateLayout>("Add New Layout", parameters);

                var modalResult = await modalInstance.Result;

                if (!modalResult.Cancelled)
                {
                    await AddNewLayout();

                }
                else
                {
                    await LoadLayouts();
                }

            }

            protected async Task CloseModal()
            {
                await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
            }

            private async Task RefreshUIAfterDeletingLayouts(List<LayoutDto> deletedLayout)
            {

                _Layouts = _Layouts.Except(deletedLayout).ToList();
                string successMessage = layoutService.GetSuccessMessage();
                ShowSuccessMessage(successMessage);
                await LoadLayouts();
            }
            protected async Task DeleteSelected()
            {
                var selectedLayouts = _Layouts?.Where(c => c.IsSelected).ToList();

                if (selectedLayouts != null && selectedLayouts.Any())
                {
                    SelectedLayouts = selectedLayouts;
                    var parameters = new ModalParameters();
                    parameters.Add("SelectedLayoutNames", SelectedLayoutNames);

                    var modalInstance = Modal.Show<DeleteLayout>("Delete Confirmation", parameters);
                    var modalResult = await modalInstance.Result;

                    if (!modalResult.Cancelled)
                    {
                        await Delete();
                        await RefreshUIAfterDeletingLayouts(selectedLayouts);
                    }
                }
                else
                {
                    ShowErrorMessage("No layouts selected for deletion.");
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
                    if (layoutService != null)
                    {

                        foreach (var layout in SelectedLayouts)
                        {
                            await layoutService.DeleteLayout(layout.LayoutId);
                        }
                        await BlazoredModalInstance.CloseAsync();

                    }
                    else
                    {
                        ShowErrorMessage("layoutService is null.");
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine($"An error occurred during deletion: {ex.Message}");

                }

            }







        [Parameter] public LayoutDto LayoutToEdit { get; set; } = new LayoutDto();
            protected bool ShowConfirmationForm { get; set; } = false;


            // Edit category
            protected async Task ShowEditModal()
            {
                var selectedLayout = _Layouts.FirstOrDefault(c => c.IsSelected);
                if (IsSingleLayoutSelected)
                {
                    LayoutToEdit = selectedLayout;


                    var parameters = new ModalParameters();
                    parameters.Add("LayoutToEdit", LayoutToEdit); // Pass the category to edit to the modal

                    var modalInstance = Modal.Show<EditLayout>("Edit Layout", parameters);
                    var modalResult = await modalInstance.Result;

                    if (!modalResult.Cancelled)
                    {
                        await UpdateLayout();


                    }
                }
                else
                {
                    ShowErrorMessage("Please select a single layout for editing.");
                }
            }

            protected async Task UpdateLayout()
            {
                try
                {

                    var result = await layoutService.UpdateLayout(LayoutToEdit);

                    if (result)
                    {

                        string successMessage = layoutService.GetSuccessMessage();
                        ShowSuccessMessage(successMessage);
                        await RefreshUIAfterUpdatingLayouts();

                    }
                    else
                    {
                        string errorMessage = layoutService.GetErrorMessage();
                        ShowErrorMessage(errorMessage);
                    }

                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");

                }

            }



            private async Task RefreshUIAfterUpdatingLayouts()
            {
                await LoadLayouts();
                await BlazoredModalInstance.CloseAsync(ModalResult.Cancel(true));
            }

    }
    
}
