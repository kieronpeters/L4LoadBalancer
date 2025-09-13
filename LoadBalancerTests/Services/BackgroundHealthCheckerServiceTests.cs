using LoadBalancer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Services
{
    public class BackgroundHealthCheckerServiceTests
    {
        private Mock<IHealthCheckerService> _mockHealthCheckerService = new();

        private BackgroudHealthCheckerService _sut;

        public BackgroundHealthCheckerServiceTests()
        {
            _sut = new BackgroudHealthCheckerService(_mockHealthCheckerService.Object);
        }

        [Fact (Skip = "cannot access the ExecuteAsyncMethod as it is protected and inherited, test the Health Checker Service instead")]
        public void ExecuteAsync_ValidServersRunning_SetsUpValidConnections()
        {
            //Arrange
            _mockHealthCheckerService.Setup(x => x.GetNextHealthyBackend()).Returns(string.Empty);

            //Act
            //var result = await _sut.ExecuteAsync(new CancellationToken();

            //Assert
            
        }

    }
}