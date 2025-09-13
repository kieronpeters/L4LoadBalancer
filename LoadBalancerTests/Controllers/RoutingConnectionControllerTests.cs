using FluentAssertions;
using LoadBalancer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Controllers
{
    public class RoutingConnectionControllerTests
    {
        private Mock<ILogger<RoutingConnectionController>> _mockLogger = new();
        private Mock<IHealthCheckerService> _mockHealthCheckerService = new();
        private Mock<IStatusReporterService> _mockStatusReporterService = new();
        private Mock<IConnectionService> _mockConnectionService = new();

        private RoutingConnectionController _sut;

        public RoutingConnectionControllerTests()
        {
            _sut = new RoutingConnectionController(_mockLogger.Object, _mockHealthCheckerService.Object, _mockStatusReporterService.Object, _mockConnectionService.Object);
        }

        [Fact]
        public async Task Connect_EmptyUrl_NotFoundStatus()
        {
            //Arrange
            _mockHealthCheckerService.Setup(x => x.GetNextHealthyBackend()).Returns(string.Empty);
            
            //Act
            var result = await _sut.Connect();

            //Assert
            _mockLogger.VerifyAll();
            _mockHealthCheckerService.VerifyAll();
            
            result.Should().NotBe(null);
            result.Should().BeOfType<NotFoundObjectResult>();
            var objectResult = (NotFoundObjectResult)result;
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Connect_ConnectionValidOrInvalid_ExpectedStatusReturned(bool connectionAllowed)
        {
            //Arrange
            var connectionUrl = "http://localhost:9001";
            _mockHealthCheckerService.Setup(x => x.GetNextHealthyBackend()).Returns(connectionUrl);
            _mockConnectionService.Setup(x => x.ConnectToServer(connectionUrl)).ReturnsAsync(connectionAllowed);

            //Act
            var result = await _sut.Connect();


            //Assert
            _mockLogger.VerifyAll();
            _mockHealthCheckerService.VerifyAll();
            _mockConnectionService.VerifyAll();
            result.Should().NotBe(null);

            if (connectionAllowed)
            {
                result.Should().BeOfType<OkObjectResult>();
                var objectResult = (OkObjectResult)result;
                objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            } else
            {
                result.Should().BeOfType<NotFoundObjectResult>();
                var objectResult = (NotFoundObjectResult)result;
                objectResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            }
                
        }

        [Fact]
        public async Task Status_NoRegisteredServers_ReturnsListEmptyAndOkStatus()
        {
            //Arrange
            _mockStatusReporterService.Setup(x => x.GetHealtyBackendStatuses()).ReturnsAsync(new List<string>());
            
            //Act
            var result = await _sut.Status();


            //Assert
            _mockLogger.VerifyAll();
            _mockStatusReporterService.VerifyAll();

            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}