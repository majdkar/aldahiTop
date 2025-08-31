using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Shared.ViewModels.Pages;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Client.Helpers;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Client.Pages.Pages
{
    public partial class Pages
    {
        private IEnumerable<PageViewModel> elements;
        private MudTable<PageViewModel> _table;
        private int totalItems;
        private string searchString = "";

        private bool loaded;
        private int clickedRowId = 0;
        private int selectedRowForTranslation = 0;
        private ClaimsPrincipal _currentUser;
        private bool _canCreatePages;
        private bool _canEditPages;
        private bool _canDeletePages;
        private bool _canSearchPages;
        private bool _canViewPages;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePages = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Pages.Create)).Succeeded;
            _canEditPages = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Pages.Edit)).Succeeded;
            _canDeletePages = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Pages.Delete)).Succeeded;
            _canSearchPages = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Pages.Search)).Succeeded;
            _canViewPages = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Pages.View)).Succeeded;

            loaded = true;
        }
        protected override async Task OnParametersSetAsync()
        {
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
            loaded = false;
        }

        private async Task<TableData<PageViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            await LoadData(state, state.Page, state.PageSize);

            return new TableData<PageViewModel>() { Items = elements, TotalItems = totalItems };
        }

        private async Task LoadData(TableState state, int pageNumber, int pageSize)
        {
            string[] orderings = null;
            state.SortLabel = "RecordOrder";
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAllPaged(EndPoints.Pages, pageNumber, pageSize, searchString, orderings);
            //if (_canViewWebSiteManagement)
            //{
                var response = await _httpClient.GetFromJsonAsync<PagedResponse<PageViewModel>>(requestUri);
                if (response != null)
                {
                    totalItems = response.TotalRecords;
                    elements = response.Items;
                    if (elements.FirstOrDefault(x => x.Id == selectedRowForTranslation) != null)
                        elements.FirstOrDefault(x => x.Id == selectedRowForTranslation).ShowTranslation = true;

                }
                else
                {
                    totalItems = 0;
                    elements = new List<PageViewModel>();
                    _snackBar.Add("Error retrieving data");
                }

            //}

        }

        private void OnSearch(string text)
        {
            //if (_canSearchWebSiteManagement)
            //{
                searchString = text;
                _table.ReloadServerData();
            //}
        }

        //private async Task InvokeAddEditModal(int id = 0)
        //{
        //    if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
        //    {
        //        var parameters = new DialogParameters();
        //        if (id != 0) // update operation
        //        {
        //            var selectedPage = elements.FirstOrDefault(c => c.Id == id);
        //            if (selectedPage != null)
        //            {
        //                parameters.Add(nameof(AddEditPageModal.PageModel), new PageUpdateModel
        //                {
        //                    Id = selectedPage.Id,
        //                    Name = selectedPage.Name,
        //                    EnglishName = selectedPage.EnglishName,
        //                    Description = selectedPage.Description,
        //                    EnglishDescription = selectedPage.EnglishDescription,
        //                    Type = selectedPage.Type,
        //                    Image = selectedPage.Image,
        //                    MenuType = selectedPage.MenuType,
        //                    GeoLocation = selectedPage.GeoLocation,
        //                    RecordOrder = selectedPage.RecordOrder,
        //                    IsActive = selectedPage.IsActive,
        //                    MenuId = selectedPage.MenuId,
        //                    Url = selectedPage.Url,

        //                });
        //            }
        //        }
        //        //add operation
        //        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = false, DisableBackdropClick = true };
        //        var dialog = _dialogService.Show<AddEditPageModal>(id == 0 ? "Create" : "Edit", parameters, options);
        //        var result = await dialog.Result;
        //        if (!result.Cancelled)
        //        {
        //            OnSearch("");
        //        }
        //    }
        //    loaded = false;
        //}

        private void InvokeAddEditModal(int id = 0)
        {
            //if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            //{
                _navigationManager.NavigateTo($"/page-details/{id}");
            //}
        }


        private void InvokeAddEditPagePhotos(int id = 0)
        {
            //if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            //{
                _navigationManager.NavigateTo($"/page-photo-details/{id}");
            //}
        }
        private void InvokeAddEditPageAttachements(int id = 0)
        {
            //if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            //{
                _navigationManager.NavigateTo($"/page-attachement-details/{id}");
            //}
        }

        private async Task InvokeTranslationModal(int pageId, int transltionId = 0)
        {
            selectedRowForTranslation = pageId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == pageId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditPageTranslationModal.PageTranslationModel), new PageTranslationUpdateModel
                    {
                        Id = selectedTranslation.Id,
                        Name = selectedTranslation.Name,
                        Description = selectedTranslation.Description,
                        Link1 = selectedTranslation.Link1,
                        Link2 = selectedTranslation.Link2,
                        Language = selectedTranslation.Language,
                        Slug = selectedTranslation.Slug,
                        PageId = selectedTranslation.PageId,
                        File = selectedTranslation.File,
                        IsActive = selectedTranslation.IsActive
                    });
                }
            }
            else//add operation
            {
                var selectedTranslation = new PageTranslationUpdateModel();
                parameters.Add(nameof(AddEditPageTranslationModal.PageTranslationModel), new PageTranslationUpdateModel
                {
                    PageId = pageId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditPageTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
            loaded = false;
        }

        protected async Task SoftDeletePage(int id)
        {
            if (_canDeletePages)
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
                    var Response = await _httpClient.DeleteAsync($"{EndPoints.Pages}/{id}");
                    if (Response.IsSuccessStatusCode)
                    {
                        _snackBar.Add("Complete Successful!", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Something went wrong!", Severity.Error);
                    }
                    await _table.ReloadServerData();
                }
            }
            loaded = false;
        }

        public void PageChanged()
        {
            _table.ReloadServerData();
        }

        private void RowClickEvent(TableRowClickEventArgs<PageViewModel> e)
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


        private async Task ChangeActive(bool isActive, PageViewModel BlockModel)
        {
            var blockUpdateModel = new PageUpdateModel
            {
                Id = BlockModel.Id,
               Description1 = BlockModel.Description1,
                Description2 = BlockModel.Description2,
                EnglishDescription1 = BlockModel.EnglishDescription1,
                EnglishDescription2 = BlockModel.EnglishDescription2,
                EnglishName = BlockModel.EnglishName,
                GeoLocation = BlockModel.GeoLocation,
                Name = BlockModel.Name,
                Image = BlockModel.Image,
                Image1 = BlockModel.Image1,
                Image2 = BlockModel.Image2,
                Image3 = BlockModel.Image3,
                Image4 = BlockModel.Image4,
                MenuId = BlockModel.MenuId,
                MenuType = BlockModel.MenuType,
                Type = BlockModel.Type,
                Url = BlockModel.Url,
                RecordOrder = BlockModel.RecordOrder,
                IsActive = isActive
            };
            var content = HelperMethods.ToJson(blockUpdateModel);
            var response = await _httpClient.PutAsync($"{EndPoints.Pages}/{BlockModel.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Updated Successful!", Severity.Success);
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            await _table.ReloadServerData();
            loaded = false;
        }
    }

}
