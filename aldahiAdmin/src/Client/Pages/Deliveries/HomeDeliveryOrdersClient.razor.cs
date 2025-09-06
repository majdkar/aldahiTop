using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class HomeDeliveryOrdersClient
    {
        MudTabs tabs;
        [Parameter] public int ClientId { get; set; }
    }
}
