using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Features.Countries.Queries.GetById
{
    public class GetCountryByIdResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }
    }
}
