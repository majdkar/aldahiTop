using FirstCall.Application.Interfaces.Services;
using FirstCall.Shared.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

public class GenericPdfService : IGenericPdfService
{
    public byte[] GeneratePdf<T>(IEnumerable<T> items, List<PdfColumn<T>> columns, string title = "Report")
    {

        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);

                // Header
                page.Header().PaddingBottom(5)
                    .Text(title)
                    .FontSize(20)
                    .SemiBold().FontColor(Colors.Blue.Darken1) // لون النص أزرق
                    .AlignCenter() ;

                // Content (Table)
                page.Content()
                    .Table(table =>
                    {
                        // تعريف الأعمدة
                        table.ColumnsDefinition(cols =>
                        {
                            foreach (var _ in columns)
                                cols.RelativeColumn();
                        });

                        // الهيدر مع لون وخلفية
                        table.Header(header =>
                        {
                            foreach (var col in columns)
                            {
                                header.Cell()
                                      .Background(Colors.Grey.Lighten2) // لون الخلفية للهيدر
                                      .Border(1)                        // حدود الخلية
                                      .BorderColor(Colors.Black)
                                      .Padding(5)
                                      .Text(col.Header)
                                      .SemiBold().FontColor(Colors.Blue.Darken1) // لون نص الهيدر أزرق
                                      .AlignCenter();
                            }
                        });

                        // الصفوف
                        foreach (var item in items)
                        {
                            foreach (var col in columns)
                            {
                                table.Cell()
                                     .Border(1)
                                     .BorderColor(Colors.Black)
                                     .Padding(5)
                                     .Text(col.Selector(item)?.ToString() ?? "")
                                     .AlignCenter();
                            }
                        }
                    });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }
}