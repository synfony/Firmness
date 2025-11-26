using Firmness.Web.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Linq;

namespace Firmness.Web.Services
{
    public class PdfService
    {
        public byte[] GenerateReceipt(Sale sale)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text(text =>
                        {
                            text.Span("Recibo de Venta #").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                            text.Span(sale.Id.ToString()).FontSize(20).FontColor(Colors.Blue.Medium);
                        });

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Fecha: {sale.SaleDate:dd/MM/yyyy}");
                            if (sale.Client != null)
                            {
                                column.Item().Text($"Cliente: {sale.Client.FirstName} {sale.Client.LastName}");
                                column.Item().Text($"Documento: {sale.Client.DocumentId}");
                            }

                            column.Item().PaddingTop(20).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Producto");
                                    header.Cell().Text("Cantidad");
                                    header.Cell().Text("Precio Unitario");
                                    header.Cell().Text("Total");
                                });

                                decimal total = 0;
                                foreach (var item in sale.SaleDetails)
                                {
                                    var itemTotal = item.Quantity * item.UnitPrice;
                                    table.Cell().Text(item.Product?.Name ?? "N/A");
                                    table.Cell().Text(item.Quantity.ToString());
                                    table.Cell().Text($"${item.UnitPrice:N2}");
                                    table.Cell().Text($"${itemTotal:N2}");
                                    total += itemTotal;
                                }

                                column.Item().PaddingTop(20).AlignRight().Text($"Total: ${total:N2}");
                                column.Item().AlignRight().Text($"IVA (19%): ${total * 0.19m:N2}");
                                column.Item().AlignRight().Text(text => text.Span($"Total a Pagar: ${total * 1.19m:N2}").SemiBold());
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
