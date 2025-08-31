using Blazored.FluentValidation;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Warehouses;
using FirstCall.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class AddEditWarehousesModal
    {
        [Inject] private IWarehousesManager WarehousesManager { get; set; }

        [Parameter] public AddEditWarehousesCommand AddEditWarehousesModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool _rightToLeft = true;
        private string direction = "rtl";

        private bool isProcessing = false;
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            var response = await WarehousesManager.SaveAsync(AddEditWarehousesModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
            isProcessing = false;
        }

        protected override async Task OnInitializedAsync()
        {
            _rightToLeft = await _clientPreferenceManager.IsRTL();
            if (_rightToLeft)
                direction = "rtl";
            else
                direction = "ltr";
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
    }
}
