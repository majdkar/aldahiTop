using System;
using FirstCall.Client.Extensions;
using FirstCall.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstCall.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Shared.Wrapper;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings;
using Microsoft.JSInterop;
using System.IO;

using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Group;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Application.Features.Groups.Queries.GetAll;

using Polly;
using System.Globalization;
using ClosedXML.Excel;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Client.Infrastructure.Managers.Clients.Persons;
using FirstCall.Application.Requests.Clients.Persons;
using Org.BouncyCastle.Asn1.Ocsp;
using FirstCall.Client.Infrastructure.Managers.Products;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;

using FirstCall.Application.Requests.Products;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Kind;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Season;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using System.Threading;
using FirstCall.Application.Interfaces.Common;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Client.Pages.Reports
{
    public partial class ProductsReport
    {

        [Inject] private IProductManager ProductManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
     
        [Inject] private IGroupManager GroupManager { get; set; }
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
        [Inject] private IKindManager KindManager { get; set; }
        [Inject] private ISeasonManager SeasonManager { get; set; }

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private MudTable<GetAllPagedProductsResponse> _table;
       
        
        private List<GetAllGroupsResponse> _groups = new();
        private List<GetAllSeasonsResponse> _seasons = new();
        private List<GetAllProductCategoriesResponse> _productCategories = new();
        private List<GetAllKindsResponse> _Kinds = new();



        //private TableState tablestate = new TableState { Page = 1, PageSize = 500 };
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;


        public string Code { get; set; }
        public string ProductType { get; set; }
        public int SeasonId { get; set; }
        public int KindId { get; set; }
        public int GroupId { get; set; }
        public int WarehousesId { get; set; }
        public int ProductCategoryId { get; set; }

        public int FromQty { get; set; }
        public int ToQty { get; set; }

        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");


        public bool IsSearchAdvanced { get; set; } = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProducts;
        private bool _canEditProducts;
        private bool _canDeleteProducts;
        private bool _canExportProducts;
        private bool _canSearchProducts;
        private bool _canViewProducts;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;
            _canExportProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            _canSearchProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            _canViewProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            _loaded = true;
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }


        private async Task LoadDataAsync()
        {
            await LoadGroupsAsync();
            await LoadSeasonsAsync();
            await LoadProductCategorysAsync();
            await LoadKindsAsync();

            //await LoadData();


        }
   
        
        private async Task LoadGroupsAsync()
        {
            var data = await GroupManager.GetAllAsync();
            if (data.Succeeded)
            {
                _groups = data.Data;
            }
        }
         
        private async Task LoadKindsAsync()
        {
            var data = await KindManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Kinds = data.Data;
            }
        }
         
        private async Task LoadSeasonsAsync()
        {
            var data = await SeasonManager.GetAllAsync();
            if (data.Succeeded)
            {
                _seasons = data.Data;
            }
        }
           
        private async Task LoadProductCategorysAsync()
        {
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _productCategories = data.Data;
            }
        }





        protected override async Task OnParametersSetAsync()
        {
            StateHasChanged();
            //if (_table != null)
            //    await LoadData();
            //_loaded = true;
        }
        private async Task<TableData<GetAllPagedProductsResponse>> LoadServerData(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductsResponse>() { Items = _pagedData, TotalItems = _totalItems };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
          
            var request = new GetAllPagedProductsRequest { PageNumber = pageNumber, PageSize = pageSize, SearchString = _searchString, Orderby = null, ProductType = "B2B" };


            if (IsSearchAdvanced == true)
            {

                var data = await ProductManager.GetAllPagedSearchAdvancedProductAsync( SeasonId,KindId, GroupId, WarehousesId,ProductCategoryId,Code, FromQty, ToQty, "B2B");

                if (data.Succeeded)
                {
                    _totalItems = data.TotalCount;
                    _currentPage = data.CurrentPage;
                    _pagedData = data.Data;
                }
                else
                {
                    foreach (var message in data.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }


            }
            else
            {
                var response = new PaginatedResult<GetAllPagedProductsResponse>(new List<GetAllPagedProductsResponse>());
                response = await ProductManager.GetAllPagedAsync(request);

                if (response.Succeeded)
                {
                    _totalItems = response.TotalCount;
                    _currentPage = response.CurrentPage;
                    _pagedData = response.Data;
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



        private async Task PrintMe()
        {
            //await _jsRuntime.InvokeVoidAsync("myPrint", _navigationManager.BaseUri);

            //var request = new GetAllPagedProductsRequest { PageNumber = tablestate.Page, PageSize = tablestate.PageSize, SearchString = _searchString, Orderby = null };

            var response = await ProductManager.GetAllByForDownloadReportAsync(SeasonId,KindId,GroupId,WarehousesId,ProductCategoryId,Code,FromQty,ToQty, "B2B");

            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("open", response.Data, "_blank");
            }

        }



        private async Task OnSearch(string text)
        {
            IsSearchAdvanced = false;
            SeasonId = 0;
            GroupId = 0;
            WarehousesId = 0;
            ProductCategoryId = 0;
            KindId = 0;
            FromQty = 0;
            ToQty = 0;
            Code = null;
            _searchString = text;
            //await LoadData();
        }

        private async Task SearchAdvance()
        {
            IsSearchAdvanced = true;
            StateHasChanged();
           // await LoadData();
        }


    


        private async Task ExportToExcel()
        {
            if (!_pagedData.Any())
            {
                _snackBar.Add(_localizer["No Data To Export"], Severity.Normal);
                return;
            }
            var wb = new XLWorkbook();
            wb.Properties.Author = "FSIT";
            wb.Properties.Title = "Products Report";
            wb.Properties.Subject = "Products Report";

            var ws = wb.Worksheets.Add("Products");

            ws.Cell(1, 1).Value = (IsArabic ? "الفئة" : "Category");
            ws.Cell(1, 2).Value = (IsArabic ? "النوع" : "Kind");
            ws.Cell(1, 3).Value = (IsArabic ? "الموسم" : "Seasson");
            ws.Cell(1, 4).Value = (IsArabic ? "الكروب" : "Group");
            ws.Cell(1, 5).Value = (IsArabic ? "الكود" : "Code");
            ws.Cell(1, 6).Value = (IsArabic ? "الكمية" : "Qty");
           


            ws.Cell(1,1).Style.Fill.BackgroundColor = XLColor.SkyBlue;
            ws.Cell(1,2).Style.Fill.BackgroundColor = XLColor.SkyBlue;
            ws.Cell(1,3).Style.Fill.BackgroundColor = XLColor.SkyBlue;
            ws.Cell(1,4).Style.Fill.BackgroundColor = XLColor.SkyBlue;
            ws.Cell(1,5).Style.Fill.BackgroundColor = XLColor.SkyBlue;
            ws.Cell(1,6).Style.Fill.BackgroundColor = XLColor.SkyBlue;
          



            int rowNumber = 1;
            foreach (var row in _pagedData.ToList())
            {
                ws.Cell(rowNumber + 1, 1).Value = IsArabic ? row.ProductCategory.NameAr : row.ProductCategory.NameEn;
                ws.Cell(rowNumber + 1, 2).Value = IsArabic ? row.Kind.NameAr : row.Kind.NameEn;
                ws.Cell(rowNumber + 1, 3).Value = IsArabic ? row.Season?.NameAr : row.Season?.NameEn;
                ws.Cell(rowNumber + 1, 4).Value = IsArabic ? row.Group?.NameAr : row.Group?.NameEn;
                ws.Cell(rowNumber + 1, 5).Value = row.Code;
                ws.Cell(rowNumber + 1, 6).Value = row.Qty;
       
              

                rowNumber++;

            }
            ws.Columns().AdjustToContents();
            ws.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cells().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            MemoryStream XLSStream = new();
            wb.SaveAs(XLSStream);


            var data = Convert.ToBase64String(XLSStream.ToArray());

            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = data,
                FileName = $"Products_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            _snackBar.Add(_localizer["Products Report exported"], Severity.Success);
        }

       


       

        

        
    }
}
