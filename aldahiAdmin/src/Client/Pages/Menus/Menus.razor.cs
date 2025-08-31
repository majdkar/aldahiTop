using FirstCall.Client.Helpers;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Shared.ViewModels.Menus;
using FirstCall.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Menus
{
    public partial class Menus
    {
        [Parameter]
        public string CategoryId { get; set; } = "0";

        public int MenuId { get; set; } = 0;


        public int id = 0;

        private IEnumerable<MenuViewModel> elements;

        private MudTable<MenuViewModel> _table;
        private int totalItems;
        private string searchString = "";
        private bool loaded;
        private int clickedRowId = 0;
        private int selectedRowForTranslation = 0;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateMenu;
        private bool _canDeleteMenu;

        protected override void OnInitialized()
        {
            loaded = true;
        }
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateMenu = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Menues.Create)).Succeeded;
            _canDeleteMenu = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Menues.Delete)).Succeeded;
            loaded = true;
        }

        protected override async Task OnParametersSetAsync()
        {
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }

        private async Task<TableData<MenuViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            await LoadData(state, MenuId, state.Page, state.PageSize);

            return new TableData<MenuViewModel>() { Items = elements, TotalItems = totalItems };
        }

        private async Task LoadData(TableState state, int MenuId, int pageNumber, int pageSize)
        {
            string[] orderings = null;
            var requestUri = "";
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            if (MenuId == 0)
            {
                //requestUri = EndPoints.GetAll(EndPoints.MenusMaster, searchString, orderings) + $"?categoryId={CategoryId}";
                requestUri = EndPoints.GetAllPaged(EndPoints.MenusMaster, pageNumber, pageSize, searchString, orderings) + $"&categoryId={CategoryId}";

            }

            else
            {
                requestUri = EndPoints.GetAll(EndPoints.MenuSub, searchString, orderings) + $"?categoryId={CategoryId}" + $"&menuid={MenuId}";
                //requestUri = EndPoints.GetAllPaged(EndPoints.MenuSub, pageNumber, pageSize, searchString, orderings) + $"?categoryId={CategoryId}" + $"&menuid={MenuId}";

            }


            var response = await _httpClient.GetFromJsonAsync<PagedResponse<MenuViewModel>>(requestUri);
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

        //private async Task InvokeAddEditModal(int id = 0)
        //{
        //    var parameters = new DialogParameters();
        //    if (id != 0) // update operation
        //    {
        //        var selectedMenu = elements.FirstOrDefault(c => c.Id == id);
        //        if (selectedMenu != null)
        //        {
        //            parameters.Add(nameof(AddEditMenuModal.MenuModel), new MenuUpdateModel
        //            {
        //                Id = selectedMenu.Id,
        //                Name = selectedMenu.Name,
        //                EnglishName = selectedMenu.EnglishName,
        //                Description = selectedMenu.Description,
        //                EnglishDescription = selectedMenu.EnglishDescription,
        //                Type = selectedMenu.Type,
        //                File = selectedMenu.File,
        //                Image = selectedMenu.Image,
        //                PageUrl = selectedMenu.PageUrl,
        //                CategoryId = selectedMenu.CategoryId,
        //                ParentId = selectedMenu.ParentId,
        //                LevelOrder = selectedMenu.LevelOrder,
        //                IsActive = selectedMenu.IsActive
        //            });
        //        }
        //    }
        //    else
        //    {
        //        parameters.Add(nameof(AddEditMenuModal.MenuModel), new MenuUpdateModel
        //        {
        //            CategoryId = int.Parse(CategoryId),
        //        });
        //    }
        //    //add operation
        //    var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = false, DisableBackdropClick = true };
        //    var dialog = _dialogService.Show<AddEditMenuModal>(id == 0 ? "Create" : "Edit", parameters, options);
        //    var result = await dialog.Result;
        //    if (!result.Cancelled)
        //    {
        //        OnSearch("");
        //    }
        //    loaded = false;
        //}


        private void InvokeAddEditModal(int id = 0)
        {
            //if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            //{
            _navigationManager.NavigateTo($"/menu-details/{id}");


            //}
        }


        private async void InvokeMenuSubModal(int id = 0)
        {
            MenuId = id;
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }

        private async void InvokeBackModal(int id)
        {
            MenuId = 0;
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }


        private async Task InvokeTranslationModal(int menuId, int transltionId = 0)
        {
            selectedRowForTranslation = menuId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == menuId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditMenuTranslationModal.MenuTranslationModel), new MenuTranslationUpdateModel
                    {
                        Id = selectedTranslation.Id,
                        Name = selectedTranslation.Name,
                        HtmlText = selectedTranslation.HtmlText,
                        Language = selectedTranslation.Language,
                        MenueId = selectedTranslation.MenueId,
                        IsActive = selectedTranslation.IsActive,
                        CategoryId = selectedTranslation.CategoryId,
                    });
                }
            }
            else//add operation
            {
                var selectedTranslation = new MenuTranslationUpdateModel();
                parameters.Add(nameof(AddEditMenuTranslationModal.MenuTranslationModel), new MenuTranslationUpdateModel
                {
                    MenueId = menuId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditMenuTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
            loaded = false;
        }

        protected async Task SoftDeleteMenu(int id)
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
                var response = await _httpClient.DeleteAsync($"{EndPoints.Menus}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Complete Successful!", Severity.Success);


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

        private void RowClickEvent(TableRowClickEventArgs<MenuViewModel> e)
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

