using FirstCall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Interfaces.Services
{

    public interface IGenericPdfService
    {
        byte[] GeneratePdf<T>(IEnumerable<T> items, List<PdfColumn<T>> columns, string title = "Report");

    }
}