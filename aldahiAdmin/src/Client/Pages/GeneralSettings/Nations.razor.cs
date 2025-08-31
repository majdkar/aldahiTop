using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Client.Extensions;
using FirstCall.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstCall.Application.Features.Nations.Commands.AddEdit;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Nation;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Nations
    {
        [Inject] private INationManager NationManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllNationsResponse> _nationList = new();
        private GetAllNationsResponse _nation = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateNations;
        private bool _canEditNations;
        private bool _canDeleteNations;
        private bool _canExportNations;
        private bool _canSearchNations;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateNations = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Nations.Create)).Succeeded;
            _canEditNations = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Nations.Edit)).Succeeded;
            _canDeleteNations = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Nations.Delete)).Succeeded;
            _canExportNations = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Nations.Export)).Succeeded;
            _canSearchNations = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Nations.Search)).Succeeded;

            await GetNationsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetNationsAsync()
        {
            var response = await NationManager.GetAllAsync();
            if (response.Succeeded)
            {
                _nationList = response.Data.ToList();
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
                var response = await NationManager.DeleteAsync(id);
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
            var response = await NationManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Nations).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Nations exported"]
                    : _localizer["Filtered Nations exported"], Severity.Success);
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
                _nation = _nationList.FirstOrDefault(c => c.Id == id);
                if (_nation != null)
                {
                    parameters.Add(nameof(AddEditNationModal.AddEditNationModel), new AddEditNationCommand
                    {
                        Id = _nation.Id,
						Name = _nation.Name,
                        Description = _nation.Description,
						ArabicName = _nation.ArabicName,Code = _nation.Code,PhoneCode = _nation.PhoneCode,

                        //Tax = _nation.Tax
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditNationModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _nation = new GetAllNationsResponse();
            await GetNationsAsync();
        }

        private bool Search(GetAllNationsResponse nation)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
			if (nation.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (nation.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
			if (nation.ArabicName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}
if (nation.Code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}
if (nation.PhoneCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}

            /**/
            return false;
        }
    }
}