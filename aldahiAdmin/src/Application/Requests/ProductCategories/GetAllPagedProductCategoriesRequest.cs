using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.ProductCategories
{
    public class GetAllPagedProductCategoriesRequest : PagedRequest
    {
        public string Type { get; set; } = "";
        public string SearchString { get; set; }
    }
}
