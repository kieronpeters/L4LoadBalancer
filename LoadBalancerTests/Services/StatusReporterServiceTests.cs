using FluentAssertions;
using LoadBalancer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Services
{
    public class StatusReporterServiceTests
    {

        private Mock<HttpClient> _mockHttpClient = new();
        private Mock<IHealthCheckerService> _mockHealthCheckerService = new();

        private StatusReporterService _sut;

        public StatusReporterServiceTests()
        {
            _sut = new StatusReporterService(_mockHttpClient.Object, _mockHealthCheckerService.Object);
        }

        [Fact]
        public async Task GetHealtyBackendStatuses_ThrowsException_EmptyListResultOfStatus()
        {
            //Arrange
            _mockHealthCheckerService.Setup(x => x.GetAllHealthyBackends()).Throws(new Exception());

            //Act
            var result = await _sut.GetHealtyBackendStatuses();

            //Assert
            _mockHttpClient.VerifyAll();
            _mockHealthCheckerService.VerifyAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<string>>();
            result.Count.Should().Be(0);

        }

        [Fact]
        public async Task GetHealtyBackendStatuses_NoHealthyInstance_EmptyListResultOfStatus()
        {
            //Arrange
            _mockHealthCheckerService.Setup(x => x.GetAllHealthyBackends()).Returns(new List<string>());

            //Act
            var result = await _sut.GetHealtyBackendStatuses();

            //Assert
            _mockHttpClient.VerifyAll();
            _mockHealthCheckerService.VerifyAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<string>>();
            result.Count.Should().Be(0);

        }

        
        [Fact(Skip = "need to mock http response content to get list correct")]
        public async Task GetHealtyBackendStatuses_OneHealthyInstance_ListWithOneResultOfStatus()
        {
            //Arrange
            _mockHealthCheckerService.Setup(x => x.GetAllHealthyBackends()).Returns(new List<string>() { "http://localhost:9001"});
            //_mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage());

            //Act
            var result = await _sut.GetHealtyBackendStatuses();

            //Assert
            _mockHttpClient.VerifyAll();
            _mockHealthCheckerService.VerifyAll();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<string>>();
            result.Count.Should().Be(1);
        }
    }
}