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
using FirstCall.Domain.Entities.Products;
using System.Linq;
using FirstCall.Client.Shared.Dialogs;

namespace FirstCall.Client.Pages.Content
{
    public partial class DashboardProduct
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private  List<DashboardDataProductResponse> _dataProducts = new();
        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");

        private bool _loaded;

        private bool isExactMatch = true; 

        private string? selectedKind;
        private string? searchCode;

        private List<DashboardDataProductResponse> filteredProducts => _dataProducts
          .Where(x => string.IsNullOrEmpty(selectedKind) ||
                      string.Equals((IsArabic ? x.KindName : x.KindNameEn)?.Trim(),
                                    selectedKind?.Trim(),
                                    StringComparison.OrdinalIgnoreCase))
            .Where(p => selectedTypesDict.Any(t => t.Value && p.Type == t.Key)
                || !selectedTypesDict.Any(t => t.Value))
          .Where(x => string.IsNullOrEmpty(searchCode) ||
              (isExactMatch
                  ? string.Equals(x.Code?.Trim(), searchCode?.Trim(), StringComparison.OrdinalIgnoreCase)
                  : x.Code?.Trim().Contains(searchCode?.Trim() ?? "", StringComparison.OrdinalIgnoreCase) == true))
          .ToList();




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


        private Dictionary<string, bool> selectedTypesDict = new()
            {
                { "B2B", false },
                { "B2C", false }
   
            };


        private void FilterByKind(string? kind)
        {
            selectedKind = kind; 
            if (string.IsNullOrEmpty(kind))
            {
                searchCode = null;
            }
        }
        void OpenImageDialog(string imageUrl)
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true };
            _dialogService.Show<ImageDialog>("Image Preview", new DialogParameters { ["ImagePath"] = imageUrl }, options);
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