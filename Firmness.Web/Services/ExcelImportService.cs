using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Firmness.Web.Data;
using Firmness.Web.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace Firmness.Web.Services
{
    public class ExcelImportService
    {
        private readonly ApplicationDbContext _context;

        public ExcelImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> ImportData(Stream stream)
        {
            var errorLog = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                var headers = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Value.ToString().Trim().ToLower());
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    var rowData = new Dictionary<string, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowData[headers[col - 1]] = worksheet.Cells[row, col].Value?.ToString().Trim();
                    }

                    await ProcessRow(rowData, row, errorLog);
                }
            }

            return errorLog;
        }

        private async Task ProcessRow(Dictionary<string, string> rowData, int rowNum, List<string> errorLog)
        {
            if (IsClientData(rowData))
            {
                await ProcessClient(rowData, rowNum, errorLog);
            }
            else if (IsProductData(rowData))
            {
                await ProcessProduct(rowData, rowNum, errorLog);
            }
            else if (IsSaleData(rowData))
            {
                await ProcessSale(rowData, rowNum, errorLog);
            }
            else
            {
                errorLog.Add($"Fila {rowNum}: No se pudo determinar el tipo de datos.");
            }
        }

        private bool IsClientData(Dictionary<string, string> rowData)
        {
            return rowData.ContainsKey("firstname") && rowData.ContainsKey("lastname");
        }

        private bool IsProductData(Dictionary<string, string> rowData)
        {
            return rowData.ContainsKey("name") && rowData.ContainsKey("price");
        }

        private bool IsSaleData(Dictionary<string, string> rowData)
        {
            return rowData.ContainsKey("saledate") && rowData.ContainsKey("clientid");
        }

        private async Task ProcessClient(Dictionary<string, string> rowData, int rowNum, List<string> errorLog)
        {
            if (!rowData.TryGetValue("documentid", out var documentId) || string.IsNullOrWhiteSpace(documentId))
            {
                errorLog.Add($"Fila {rowNum}: El DocumentId del cliente es obligatorio.");
                return;
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.DocumentId == documentId);
            bool isNew = client == null;
            client ??= new Client { DocumentId = documentId, PersonType = "Client" };

            client.FirstName = rowData.GetValueOrDefault("firstname");
            client.LastName = rowData.GetValueOrDefault("lastname");
            client.Address = rowData.GetValueOrDefault("address");
            client.PhoneNumber = rowData.GetValueOrDefault("phonenumber");

            if (isNew)
            {
                _context.Clients.Add(client);
            }
            else
            {
                _context.Clients.Update(client);
            }
            await _context.SaveChangesAsync();
        }

        private async Task ProcessProduct(Dictionary<string, string> rowData, int rowNum, List<string> errorLog)
        {
            if (!rowData.TryGetValue("name", out var name) || string.IsNullOrWhiteSpace(name))
            {
                errorLog.Add($"Fila {rowNum}: El nombre del producto es obligatorio.");
                return;
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
            bool isNew = product == null;
            product ??= new Product { Name = name };

            product.Description = rowData.GetValueOrDefault("description");
            if (decimal.TryParse(rowData.GetValueOrDefault("price"), out var price))
            {
                product.Price = price;
            }
            if (int.TryParse(rowData.GetValueOrDefault("stock"), out var stock))
            {
                product.Stock = stock;
            }

            if (isNew)
            {
                _context.Products.Add(product);
            }
            else
            {
                _context.Products.Update(product);
            }
            await _context.SaveChangesAsync();
        }

        private async Task ProcessSale(Dictionary<string, string> rowData, int rowNum, List<string> errorLog)
        {
            if (!DateTime.TryParse(rowData.GetValueOrDefault("saledate"), out var saleDate))
            {
                errorLog.Add($"Fila {rowNum}: Fecha de venta inválida.");
                return;
            }
            if (!int.TryParse(rowData.GetValueOrDefault("clientid"), out var clientId))
            {
                errorLog.Add($"Fila {rowNum}: ID de cliente inválido.");
                return;
            }

            var sale = new Sale
            {
                SaleDate = saleDate,
                ClientId = clientId
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }
    }
}
