using Firmness.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Linq;

namespace Firmness.Core.Services
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
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Producto");
                                    header.Cell().AlignRight().Text("Cantidad");
                                    header.Cell().AlignRight().Text("Precio Unitario");
                                    header.Cell().AlignRight().Text("Total");
                                });

                                decimal subtotal = 0;
                                if (sale.SaleDetails != null)
                                {
                                    foreach (var item in sale.SaleDetails)
                                    {
                                        var itemTotal = item.Quantity * item.UnitPrice;
                                        table.Cell().Text(item.Product?.Name ?? "N/A");
                                        table.Cell().AlignRight().Text(item.Quantity.ToString());
                                        table.Cell().AlignRight().Text($"${item.UnitPrice:N2}");
                                        table.Cell().AlignRight().Text($"${itemTotal:N2}");
                                        subtotal += itemTotal;
                                    }
                                }

                                const decimal ivaRate = 0.19m;
                                var iva = subtotal * ivaRate;
                                var grandTotal = subtotal + iva;

                                table.Cell().ColumnSpan(3).AlignRight().Text("Subtotal");
                                table.Cell().AlignRight().Text($"${subtotal:N2}");

                                table.Cell().ColumnSpan(3).AlignRight().Text($"IVA ({ivaRate:P0})");
                                table.Cell().AlignRight().Text($"${iva:N2}");

                                table.Cell().ColumnSpan(3).AlignRight().Text(text => text.Span("Total a Pagar").SemiBold());
                                table.Cell().AlignRight().Text(text => text.Span($"${grandTotal:N2}").SemiBold());
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
