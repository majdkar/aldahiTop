using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared.ViewModels.Events;
using FirstCall.Shared.Wrapper;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace FirstCall.Client.Pages.Events
{
    public partial class Events
    {
        private string categoryId = "N/A"; // Query string parameter

        private IEnumerable<EventViewModel> elements;
        private IEnumerable<EventCategoryViewModel> categories;
        private MudTable<EventViewModel> _table;
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
            var isRedirectedFromEventsCategriesPage = _navigationManager.TryGetQueryString<string>("categoryId", out categoryId);
            if (!isRedirectedFromEventsCategriesPage)
                categoryId = "0";
            await LoadCategories();
            await _table.ReloadServerData();
        }

        private async Task<TableData<EventViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            await LoadData(state, state.Page, state.PageSize);

            return new TableData<EventViewModel>() { Items = elements, TotalItems = totalItems };
        }
        private void InvokeAddEditEventPhotos(int id = 0)
        {
            if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            {
                _navigationManager.NavigateTo($"/event-photo-details/{id}");
            }
        }
        private void InvokeAddEditEventAttachements(int id = 0)
        {
            if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            {
                _navigationManager.NavigateTo($"/event-attachement-details/{id}");
            }
        }
        private async Task LoadData(TableState state, int pageNumber, int pageSize)
        {
            string[] orderings = null;
            state.SortLabel = "RecordOrder";
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAllPaged(EndPoints.Events, pageNumber, pageSize, searchString, orderings) + $"&categoryId={categoryId}";
            if (_canViewWebSiteManagement)
            {
                var response = await _httpClient.GetFromJsonAsync<PagedResponse<EventViewModel>>(requestUri);
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
        }

        private async Task LoadCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<EventCategoryViewModel>>(EndPoints.EventCategoriesSelect);
            if (response != null)
            {
                categories = response;
            }
            else
            {
                _snackBar.Add("Error retrieving data");
            }
            loaded = false;
        }

        private void InvokeAddEditEvent(int id = 0)
        {
            if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            {
                _navigationManager.NavigateTo($"/event-details/{id}");
            }
        }

        private async Task InvokeTranslationModal(int eventId, int transltionId = 0)
        {
            selectedRowForTranslation = eventId;
            var parameters = new DialogParameters();
            if (transltionId != 0) // update operation
            {
                var selectedTranslation = elements.FirstOrDefault(c => c.Id == eventId).Translations.FirstOrDefault(t => t.Id == transltionId);
                if (selectedTranslation != null)
                {
                    parameters.Add(nameof(AddEditEventTranslationModal.EventTranslationModel), new EventTranslationUpdateModel
                    {
                        Id = selectedTranslation.Id,
                        Name = selectedTranslation.Name,
                        
                        Place = selectedTranslation.Place,
                        Description = selectedTranslation.Description,
                        Language = selectedTranslation.Language,
                        Slug = selectedTranslation.Slug,
                        EventId = selectedTranslation.EventId,
                        IsActive = selectedTranslation.IsActive
                    });
                }
            }
            else//add operation
            {
                var selectedTranslation = new EventTranslationUpdateModel();
                parameters.Add(nameof(AddEditEventTranslationModal.EventTranslationModel), new EventTranslationUpdateModel
                {
                    EventId = eventId
                });
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditEventTranslationModal>(transltionId == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
                loaded = false;
            }
        }

        protected async Task SoftDeleteEvent(int id)
        {
            if (_canDeleteWebSiteManagement)
            {
                var result = await _httpClient.DeleteAsync($"{EndPoints.Events}/{id}");
                if (result.IsSuccessStatusCode)
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

        public void PageChanged()
        {
            _table.ReloadServerData();
        }

        private void RowClickEvent(TableRowClickEventArgs<EventViewModel> e)
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
