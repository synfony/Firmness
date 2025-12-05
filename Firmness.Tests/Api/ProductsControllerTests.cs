using AutoMapper;
using Firmness.API.Controllers;
using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Firmness.Tests.Api
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;

        public ProductsControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
        }

        private DbContextOptions<ApplicationDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedDatabase(DbContextOptions<ApplicationDbContext> options)
        {
            await using var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            context.Products.AddRange(
                new Product { Id = 1, Name = "Test Product 1", Price = 10 },
                new Product { Id = 2, Name = "Test Product 2", Price = 20 }
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var dbOptions = GetDbOptions();
            await SeedDatabase(dbOptions);
            
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns((IEnumerable<Product> src) => src.Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description, Stock = p.Stock }).ToList());

            // Act
            await using var context = new ApplicationDbContext(dbOptions);
            var controller = new ProductsController(context, _mapperMock.Object);
            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var dbOptions = GetDbOptions();
            await using (var context = new ApplicationDbContext(dbOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(new List<ProductDto>());

            // Act
            await using var actContext = new ApplicationDbContext(dbOptions);
            var controller = new ProductsController(actContext, _mapperMock.Object);
            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }
    }
}
