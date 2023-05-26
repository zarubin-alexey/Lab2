using Lab2.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Lab2.Tests
{
    public class OrdersApiControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient httpClient;

        public OrdersApiControllerTests(WebApplicationFactory<Program> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ShouldCreate()
        {
            // Arrange
            var order = new Order()
            {
                Id = 5,
                UserId = 3,
                ProductId = 2,
                Count = 2,
                Address = "Some Address",
                Phone = "123456789",
                Created = DateTime.Now
            };

            // Act
            var response = await httpClient.PostAsJsonAsync("api/orders", order);

            // Assert
            Assert.Equal(response.StatusCode, HttpStatusCode.Created);
        }
    }
}
