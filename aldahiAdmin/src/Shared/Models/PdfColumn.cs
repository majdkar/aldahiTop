using System;

namespace FirstCall.Shared.Models
{
    public class PdfColumn<T>
    {
        public string Header { get; set; } = string.Empty;
        public Func<T, object?> Selector { get; set; } = _ => null;
    }
}