using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Shared.Wrapper;
using FirstCall.Client.Helpers;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Client.Extensions;

namespace FirstCall.Client.Pages.Blocks

{
    public partial class BlockCategories
    {
        private IEnumerable<BlockCategoryViewModel> elements;
        private MudTable<BlockCategoryViewModel> _table;
        private int totalItems;
        private string searchString = "";
        private bool loaded;
        private int clickedRowId = 0;
        private int selectedRowForTranslation = 0;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateWebSiteManagement;
        private bool _canEditWebSiteManagement;
        private bool _canDeleteWebSiteManagement;
        private bool _canSearchWebSiteManagement;
        private bool _canViewWebSiteManagement;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.Create)).Succeeded;
            _canEditWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.Edit)).Succeeded;
            _canDeleteWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.Delete)).Succeeded;
            _canSearchWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.Search)).Succeeded;
            _canViewWebSiteManagement = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WebSiteManagement.View)).Succeeded;

            loaded = true;
        }
        protected override async Task OnParametersSetAsync()
        {
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }

        private async Task<TableData<BlockCategoryViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            await LoadData(state, state.Page, state.PageSize);

            loaded = false;
            return new TableData<BlockCategoryViewModel>() { Items = elements, TotalItems = totalItems };

        }

        private async Task LoadData(TableState state, int pageNumber, int pageSize)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAllPaged(EndPoints.BlockCategories, pageNumber, pageSize, searchString, orderings);
            if (_canViewWebSiteManagement)
            {
                var response = await _httpClient.GetFromJsonAsync<PagedResponse<BlockCategoryViewModel>>(requestUri);
                if (response != null)
                {
                    
                    totalItems = response.TotalRecords;
                    elements = response.Items;
                    if (elements.FirstOrDefault(x => x.Id == selectedRowForTranslation) != null)
                        elements.FirstOrDefault(x => x.Id == selectedRowForTranslation).ShowTranslation = true;
                }
                else
                {
                    _snackBar.Add("Error retrieving data");
                }
            }
        }

        private void OnSearch(string text)
        {
            if (_canSearchWebSiteManagement)
            {
                searchString = text;
                _table.ReloadServerData();
            }
            loaded = false;
        }


        private async Task InvokeAddEditModal(int id = 0)
        {
            if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            {
                var parameters = new DialogParameters();
                if (id != 0) // update operation
                {
                    var selectedBlockCategory = elements.FirstOrDefault(c => c.Id == id);
                    if (selectedBlockCategory != null)
                    {
                        parameters.Add(nameof(AddEditBlockCategoryModal.BlockCategoryModel), new BlockCategoryUpdateModel
                        {
                            Id = selectedBlockCategory.Id,
                            Name = selectedBlockCategory.Name,
                            Description = selectedBlockCategory.Description,
                            EnglishName = selectedBlockCategory.EnglishName,
                            EnglishDescription = selectedBlockCategory.EnglishDescription,
                            BlockType = selectedBlockCategory.BlockType,
                            IsActive = selectedBlockCategory.IsActive
                        });
                    }
                }
                //add operation
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
                var dialog = _dialogService.Show<AddEditBlockCategoryModal>(id == 0 ? "Create" : "Edit", parameters, options);
                var result = await dialog.Result;
                if (!result.Cancelled)
                {
                    OnSearch("");
                }
            }
            loaded = false;
        }

        private async Task InvokeTranslationModal(int blockCategoryId, int transltionId = 0)
        {
            selectedRowForTranslation = blockCategoryId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == blockCategoryId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditBlockCategoryTranslationModal.BlockCategoryTranslationModel), new BlockCategoryTranslationUpdateModel
                    {
                        Id = selectedTranslation.Id,
                        Name = selectedTranslation.Name,
                        Description = selectedTranslation.Description,
                        Language = selectedTranslation.Language,
                        Slug = selectedTranslation.Slug,
                        CategoryId = selectedTranslation.CategoryId,
                        IsActive = selectedTranslation.IsActive
                    });
                }
            }
            else//add operation
            {
                var selectedTranslation = new BlockCategoryTranslationUpdateModel();
                parameters.Add(nameof(AddEditBlockCategoryTranslationModal.BlockCategoryTranslationModel), new BlockCategoryTranslationUpdateModel
                {
                    CategoryId = blockCategoryId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBlockCategoryTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
            loaded = false;
        }

        protected async Task SoftDeleteBlockCategory(int id)
        {
            if (_canDeleteWebSiteManagement)
            {

                string deleteContent = localizer["Delete Content"];
                var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
                var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
                var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["Delete"], parameters, options);
                var result = await dialog.Result;
                if (!result.Cancelled)
                {
                    var response = await _httpClient.DeleteAsync($"{EndPoints.BlockCategories}/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        _snackBar.Add("Complete Successful!", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Something went wrong!", Severity.Error);
                    }
                    await _table.ReloadServerData();
                }
                loaded = false;
            }
        }
        public void PageChanged()
        {
            _table.ReloadServerData();
            loaded = false;
        }

        private void RowClickEvent(TableRowClickEventArgs<BlockCategoryViewModel> e)
        {
            if (!e.Item.ShowTranslation)
            {
                if (clickedRowId != 0)
                    elements.FirstOrDefault(x => x.Id == clickedRowId).ShowTranslation = false;
                e.Item.ShowTranslation = true;
                clickedRowId = e.Item.Id;
            }
            else
            {
                e.Item.ShowTranslation = false;
            }
            loaded = false;
        }

    }

}
