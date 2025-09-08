using FirstCall.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using FirstCall.Domain.Entities;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Products.Queries.GetAllPaged
{
    public class PdfResponse
    {
        public string PdfUrl { get; set; } = string.Empty;


    }
}