using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class HomeB2CDeliveryOrdersByClient
    {
        MudTabs tabs;
        [Parameter] public int ClientId { get; set; }
    }
}
