using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Clients.Persons
{
    public class GetAllPagedPersonsRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
  
    }
}
