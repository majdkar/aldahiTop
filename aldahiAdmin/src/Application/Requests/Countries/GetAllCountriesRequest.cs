namespace FirstCall.Application.Requests.Countries
{
    public class GetAllCountriesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
