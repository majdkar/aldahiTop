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
                    .SemiBold().FontColor(Colors.Blue.Darken1)
                    .AlignCenter();

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

                        // الهيدر
                        table.Header(header =>
                        {
                            foreach (var col in columns)
                            {
                                header.Cell()
                                      .Background(Colors.Grey.Lighten2)
                                      .Border(1)
                                      .BorderColor(Colors.Black)
                                      .Padding(5)
                                      .Text(col.Header)
                                      .SemiBold().FontColor(Colors.Blue.Darken1)
                                      .AlignCenter();
                            }
                        });

                        // الصفوف مع صف فاصل عند تغيير الكود
                        string lastCode = null;

                        foreach (var item in items)
                        {
                            // الحصول على الكود الحالي
                            var currentCode = item?.GetType().GetProperty("Code")?.GetValue(item, null)?.ToString();

                            // إذا الكود تغيّر → أضف صف فاصل
                            if (lastCode != null && currentCode != lastCode)
                            {
                                foreach (var col in columns)
                                {
                                    table.Cell()
                                         .Background(Colors.Grey.Lighten3)   // لون الصف الفاصل
                                         .Border(1)
                                         .BorderColor(Colors.Black)
                                         .Padding(5)
                                         .Text("")                           // خلية فارغة
                                         .AlignCenter();
                                }
                            }

                            // الصف العادي
                            foreach (var col in columns)
                            {
                                table.Cell()
                                     .Border(1)
                                     .BorderColor(Colors.Black)
                                     .Padding(5)
                                     .Text(col.Selector(item)?.ToString() ?? "")
                                     .AlignCenter();
                            }

                            lastCode = currentCode;
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