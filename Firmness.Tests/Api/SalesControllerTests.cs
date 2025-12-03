using AutoMapper;
using Firmness.API.Controllers;
using Firmness.Core.Data;
using Firmness.Core.Models;
using Firmness.Core.Services;
using Firmness.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Firmness.Tests.Api
{
    public class SalesControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IEmailService> _emailMock;

        public SalesControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FirmnessTestDb_Sales")
                .Options;

            _mapperMock = new Mock<IMapper>();
            _emailMock = new Mock<IEmailService>();
        }

        private async Task SeedDatabase()
        {
            await using var context = new ApplicationDbContext(_dbOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new ApplicationUser { Id = "test-user-id", Email = "test@example.com", UserName = "test@example.com" };
            context.Users.Add(user);

            var client = new Client { Id = 1, UserId = "test-user-id", FirstName = "Test", LastName = "User", User = user };
            context.Clients.Add(client);

            context.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 100 });
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task PostSale_CreatesSale_And_SendsEmail_WhenUserIsAuthenticated()
        {
            // Arrange
            await SeedDatabase();

            var saleDto = new SaleDto
            {
                SaleDetails = new List<SaleDetailDto>
                {
                    new SaleDetailDto { ProductId = 1, Quantity = 2, UnitPrice = 100 }
                }
            };

            _mapperMock.Setup(m => m.Map<SaleDto>(It.IsAny<Sale>())).Returns(saleDto);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            await using var context = new ApplicationDbContext(_dbOptions);
            var controller = new SalesController(context, _mapperMock.Object, _emailMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = user }
                }
            };

            // Act
            var result = await controller.PostSale(saleDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);

            // Verify the sale was saved to the database
            await using var assertContext = new ApplicationDbContext(_dbOptions);
            Assert.Equal(1, await assertContext.Sales.CountAsync());
            var savedSale = await assertContext.Sales.Include(s => s.SaleDetails).FirstAsync();
            Assert.Equal(1, savedSale.ClientId);
            Assert.Equal(2, savedSale.SaleDetails.First().Quantity);

            // Verify that the email service was called
            _emailMock.Verify(s => s.SendEmailAsync(
                "test@example.com",
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Once);
        }
    }
}