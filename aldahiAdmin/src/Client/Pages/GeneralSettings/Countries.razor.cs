using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Permission;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Country;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Client.Extensions;
using System.Linq;
using FirstCall.Application.Features.Countries.Queries.GetAll;
using FirstCall.Application.Features.Countries.Commands.AddEdit;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Countries
    {
        [Inject] private ICountryManager CountryManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCountriesResponse> _countryList = new();
        private GetAllCountriesResponse _country = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCountries;
        private bool _canEditCountries;
        private bool _canDeleteCountries;
        private bool _canExportCountries;
        private bool _canSearchCountries;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCountries = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Countries.Create)).Succeeded;
            _canEditCountries = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Countries.Edit)).Succeeded;
            _canDeleteCountries = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Countries.Delete)).Succeeded;
            _canExportCountries = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Countries.Export)).Succeeded;
            _canSearchCountries = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Countries.Search)).Succeeded;

            await GetCountriesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetCountriesAsync()
        {
            var response = await CountryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _countryList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await CountryManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await CountryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Countries).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Countries exported"]
                    : _localizer["Filtered Countries exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _country = _countryList.FirstOrDefault(c => c.Id == id);
                if (_country != null)
                {
                    parameters.Add(nameof(AddEditCountryModal.AddEditCountryModel), new AddEditCountryCommand
                    {
                        Id = _country.Id,
                        NameAr = _country.NameAr,
                        NameEn = _country.NameEn,
                        Code = _country.Code,
                        PhoneCode = _country.PhoneCode,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditCountryModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _country = new GetAllCountriesResponse();
            await GetCountriesAsync();
        }

        private bool Search(GetAllCountriesResponse country)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (country.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (country.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (country.Code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (country.PhoneCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            /**/
            return false;
        }
    }
}
