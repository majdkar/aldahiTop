using System.Collections.Generic;

namespace FirstCall.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataProductResponse
    {
        public int Id  { get; set; }
        public int Qty  { get; set; }
        public string Code  { get; set; }
        public string CategoryName  { get; set; }
        public string KindName  { get; set; }
        public string GroupName  { get; set; }


        public string CategoryNameEn  { get; set; }
        public string KindNameEn  { get; set; }
        public string GroupNameEn  { get; set; }


        public string ImageProduct { get; set; }
    }
}