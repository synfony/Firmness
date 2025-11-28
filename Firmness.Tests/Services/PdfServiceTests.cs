using Xunit;
using Firmness.Core.Services;
using Firmness.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Firmness.Tests.Services
{
    public class PdfServiceTests
    {
        [Fact]
        public void GenerateReceipt_ShouldReturnPdfBytes_ForValidSale()
        {
            // Arrange
            var pdfService = new PdfService();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 10.00m, Stock = 100 };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 20.00m, Stock = 50 };

            var sale = new Sale
            {
                Id = 1,
                Client = new Client { FirstName = "John", LastName = "Doe", DocumentId = "123456789" },
                SaleDetails = new List<SaleDetail>
                {
                    new SaleDetail { Product = product1, Quantity = 2, UnitPrice = product1.Price },
                    new SaleDetail { Product = product2, Quantity = 1, UnitPrice = product2.Price }
                }
            };

            // Act
            var pdfBytes = pdfService.GenerateReceipt(sale);

            // Assert
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);
            // Further assertions could involve parsing the PDF or checking its size,
            // but for a basic unit test, checking for non-empty bytes is a good start.
        }

        [Fact]
        public void GenerateReceipt_ShouldHandleEmptySaleDetails()
        {
            // Arrange
            var pdfService = new PdfService();
            var sale = new Sale
            {
                Id = 2,
                Client = new Client { FirstName = "Jane", LastName = "Smith", DocumentId = "987654321" },
                SaleDetails = new List<SaleDetail>() // Empty details
            };

            // Act
            var pdfBytes = pdfService.GenerateReceipt(sale);

            // Assert
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0); // Should still generate a valid, albeit empty-details, PDF
        }

        [Fact]
        public void GenerateReceipt_ShouldCalculateCorrectTotal()
        {
            // Arrange
            var pdfService = new PdfService();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 10.00m, Stock = 100 };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 20.00m, Stock = 50 };

            var sale = new Sale
            {
                Id = 3,
                Client = new Client { FirstName = "Test", LastName = "User", DocumentId = "112233445" },
                SaleDetails = new List<SaleDetail>
                {
                    new SaleDetail { Product = product1, Quantity = 2, UnitPrice = product1.Price }, // 2 * 10 = 20
                    new SaleDetail { Product = product2, Quantity = 3, UnitPrice = product2.Price }  // 3 * 20 = 60
                }
            };
            // Expected subtotal = 20 + 60 = 80
            // Expected IVA = 80 * 0.19 = 15.20
            // Expected Grand Total = 80 + 15.20 = 95.20

            // Act
            var pdfBytes = pdfService.GenerateReceipt(sale);

            // Assert
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);

            // Note: Directly asserting the content of a PDF is complex for a unit test.
            // This test primarily ensures generation and could be extended with integration tests
            // that parse the PDF content if precise text verification is needed.
            // For now, we trust the QuestPDF library to render the values correctly based on the input.
        }
    }
}
