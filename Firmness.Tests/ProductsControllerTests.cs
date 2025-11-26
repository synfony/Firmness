using Firmness.Web.Controllers.Admin;
using Firmness.Web.Data;
using Firmness.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Firmness.Tests
{
    public class ProductsControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            var dbContext = GetDbContext();
            dbContext.Products.Add(new Product { Name = "Test Product 1", Price = 10, Stock = 100 });
            dbContext.Products.Add(new Product { Name = "Test Product 2", Price = 20, Stock = 200 });
            await dbContext.SaveChangesAsync();

            var controller = new ProductsController(dbContext, null); // IWebHostEnvironment is not needed for this test

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}
