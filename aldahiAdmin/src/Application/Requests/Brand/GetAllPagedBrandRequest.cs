using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Brand
{
    public class GetAllPagedBrandRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
