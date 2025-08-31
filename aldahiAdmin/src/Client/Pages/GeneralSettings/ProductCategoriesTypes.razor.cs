using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class ProductCategoriesTypes
    {
        public  void Back()
        {
            //  await _jsRuntime.InvokeVoidAsync("history.back", -1);

            _navigationManager.NavigateTo($"/home");
        }

    }
}
