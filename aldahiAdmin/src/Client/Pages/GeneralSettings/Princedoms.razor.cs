using System;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Client.Extensions;
using FirstCall.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Princedoms
    {
        [Inject] private IPrincedomManager PrincedomManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllPrincedomsResponse> _princedomList = new();
        private GetAllPrincedomsResponse _princedom = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePrincedoms;
        private bool _canEditPrincedoms;
        private bool _canDeletePrincedoms;
        private bool _canExportPrincedoms;
        private bool _canSearchPrincedoms;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePrincedoms = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Princedoms.Create)).Succeeded;
            _canEditPrincedoms = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Princedoms.Edit)).Succeeded;
            _canDeletePrincedoms = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Princedoms.Delete)).Succeeded;
            _canExportPrincedoms = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Princedoms.Export)).Succeeded;
            _canSearchPrincedoms = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Princedoms.Search)).Succeeded;

            await GetPrincedomsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPrincedomsAsync()
        {
            var response = await PrincedomManager.GetAllAsync();
            if (response.Succeeded)
            {
                _princedomList = response.Data.ToList();
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
                var response = await PrincedomManager.DeleteAsync(id);
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
            var response = await PrincedomManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Princedoms).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Princedoms exported"]
                    : _localizer["Filtered Princedoms exported"], Severity.Success);
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
                _princedom = _princedomList.FirstOrDefault(c => c.Id == id);
                if (_princedom != null)
                {
                    parameters.Add(nameof(AddEditPrincedomModal.AddEditPrincedomModel), new AddEditPrincedomCommand
                    {
                        Id = _princedom.Id,
						Name = _princedom.Name,
                        Description = _princedom.Description,
						ar_title = _princedom.ar_title,en_title = _princedom.en_title,Code = _princedom.Code,

                        //Tax = _princedom.Tax
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditPrincedomModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _princedom = new GetAllPrincedomsResponse();
            await GetPrincedomsAsync();
        }

        private bool Search(GetAllPrincedomsResponse princedom)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
			if (princedom.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (princedom.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
			if (princedom.ar_title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}
if (princedom.en_title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}
if (princedom.Code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
{
return true;
}

            /**/
            return false;
        }
    }
}