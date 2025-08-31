using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FirstCall.Application.Interfaces.Services
{
    public interface IHtmlToPDFService
    {
        Task ConvertDoc(string FileName, string HtmlContent);

    }
}