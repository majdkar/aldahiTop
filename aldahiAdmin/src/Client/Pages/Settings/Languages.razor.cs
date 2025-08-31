using FirstCall.Client.Helpers;
using FirstCall.Shared.ViewModels.Settings;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Security.Claims;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Shared.ViewModels.Settings.Languages;

namespace FirstCall.Client.Pages.Settings
{
    public partial class Languages
    {
        private IEnumerable<LanguageViewModel> elements;
        private MudTable<LanguageViewModel> _table;
        private LanguageViewModel selectedItem = null;
        private LanguageViewModel elementBeforeEdit;
        private int totalItems;
        private string searchString = "";
        private bool loaded;
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

        private async Task<TableData<LanguageViewModel>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                state.Page = 0;
            }
            if (_canViewWebSiteManagement)
            {
                await LoadData(state);
            }
            return new TableData<LanguageViewModel>() { Items = elements, TotalItems = totalItems };

        }
        private async Task LoadData(TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var requestUri = EndPoints.GetAll(EndPoints.Languages, searchString, orderings);
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<LanguageViewModel>>(requestUri);
            if (response != null)
            {
                elements = response;
                totalItems = response.Count();
            }
            else
            {
                _snackBar.Add("Error retrieving data");
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


        private async Task InvokeAddEditModalAsync(int id = 0)
        {
            if ((id == 0 && _canCreateWebSiteManagement) || (id != 0 && _canEditWebSiteManagement))
            {
                var parameters = new DialogParameters();
                if (id != 0) // update operation
                {
                    var selectedLanguage = elements.FirstOrDefault(c => c.Id == id);
                    if (selectedLanguage != null)
                    {
                        parameters.Add(nameof(AddEditLanguageModal.LanguageModel), new LanguageUpdateModel
                        {
                            Id = selectedLanguage.Id,
                            Name = selectedLanguage.Name,
                            LanguageCode = selectedLanguage.LanguageCode,
                            IsActive = selectedLanguage.IsActive
                        });
                    }
                }
                //add operation
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = false, DisableBackdropClick = true };
                var dialog = _dialogService.Show<AddEditLanguageModal>(id == 0 ? "Create" : "Edit", parameters, options);
                var result = await dialog.Result;
                if (!result.Cancelled)
                {
                    OnSearch("");
                }
            }
            loaded = false;
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            {
                Name = ((LanguageViewModel)element).Name,
                LanguageCode = ((LanguageViewModel)element).LanguageCode,
                IsActive = ((LanguageViewModel)element).IsActive
            };
            loaded = false;
        }


        private async Task Save(LanguageViewModel element)
        {
            var content = HelperMethods.ToJson(element);
            HttpResponseMessage response;
            response = await _httpClient.PutAsync($"{EndPoints.Languages}/{element.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
        }



        private void ResetItemToOriginalValues(object element)
        {
            ((LanguageViewModel)element).LanguageCode = elementBeforeEdit.LanguageCode;
            ((LanguageViewModel)element).Name = elementBeforeEdit.Name;
            ((LanguageViewModel)element).IsActive = elementBeforeEdit.IsActive;
        }
    }
}
