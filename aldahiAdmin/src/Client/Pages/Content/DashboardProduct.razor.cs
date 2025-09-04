using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FirstCall.Client.Infrastructure.Managers.Dashboard;
using FirstCall.Shared.Constants.Application;
using FirstCall.Application.Features.Dashboards.Queries.GetData;
using System.Globalization;

namespace FirstCall.Client.Pages.Content
{
    public partial class DashboardProduct
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private  List<DashboardDataProductResponse> _dataProducts = new();
        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
            HubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });
            await HubConnection.StartAsync();
        }

        private async Task LoadDataAsync()
        {
            var response = await DashboardManager.GetDataProductAsync();
            if (response.Succeeded)
            {
                _dataProducts = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }
}