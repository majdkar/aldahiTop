using FirstCall.Shared.ViewModels.Menus;
using FirstCall.Shared.Wrapper;
using FirstCall.Client.Helpers;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using FirstCall.Client.Extensions;
using FirstCall.Shared.Constants.Application;
using System.Security.Claims;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace FirstCall.Client.Pages.Menus

{
    public partial class MenuCategories
    {
        private IEnumerable<MenuCategoryViewModel> elements;
        private MudTable<MenuCategoryViewModel> _table;
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

        private HubConnection HubConnection { get; set; }
        protected async override void OnInitialized()
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

        private async Task<TableData<MenuCategoryViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            await LoadData(state);

            return new TableData<MenuCategoryViewModel>() { Items = elements, TotalItems = totalItems };
        }

        private async Task LoadData(TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAll(EndPoints.MenuCategories, searchString, orderings);
            var response = await _httpClient.GetFromJsonAsync<PagedResponse<MenuCategoryViewModel>>(requestUri);
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

        private void OnSearch(string text)
        {
            searchString = text;
            _table.ReloadServerData();
        }


        protected override async Task OnInitializedAsync()
        {

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task InvokeAddEditModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0) // update operation
            {
                var selectedMenuCategory = elements.FirstOrDefault(c => c.Id == id);
                if (selectedMenuCategory != null)
                {
                    parameters.Add(nameof(AddEditMenuCategoryModal.MenuCategoryModel), new MenuCategoryUpdateModel
                    {
                        Id = selectedMenuCategory.Id,
                        Name = selectedMenuCategory.Name,
                        Description = selectedMenuCategory.Description,
                        EnglishName = selectedMenuCategory.EnglishName,
                        EnglishDescription = selectedMenuCategory.EnglishDescription,
                        IsActive = selectedMenuCategory.IsActive,
                        IsVisableUser = selectedMenuCategory.IsVisableUser

                    });
                }
            }
            //add operation
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditMenuCategoryModal>(id == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
            loaded = false;
        }

        private async Task InvokeTranslationModal(int menuCategoryId, int transltionId = 0)
        {
            selectedRowForTranslation = menuCategoryId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == menuCategoryId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditMenuCategoryTranslationModal.MenuCategoryTranslationModel), new MenuCategoryTranslationUpdateModel
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
                var selectedTranslation = new MenuCategoryTranslationUpdateModel();
                parameters.Add(nameof(AddEditMenuCategoryTranslationModal.MenuCategoryTranslationModel), new MenuCategoryTranslationUpdateModel
                {
                    CategoryId = menuCategoryId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditMenuCategoryTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
            loaded = false;
        }

        protected async Task SoftDeleteBlock(int id)
        {
            string deleteContent = localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var respone = await _httpClient.DeleteAsync($"{EndPoints.MenuCategories}/{id}");
                if (respone.IsSuccessStatusCode)
                {
                    _snackBar.Add("Complete Successful!", Severity.Success);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);

                }
                else
                {
                    _snackBar.Add("Something went wrong!", Severity.Error);
                }
            }
            await _table.ReloadServerData();
        }

        public void PageChanged()
        {
            _table.ReloadServerData();
        }

        private void RowClickEvent(TableRowClickEventArgs<MenuCategoryViewModel> e)
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
