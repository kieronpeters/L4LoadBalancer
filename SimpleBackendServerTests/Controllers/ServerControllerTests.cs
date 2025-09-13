using Moq;
using SimpleBackendServer.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Net;


namespace ServerControllerTests;

public class UnitTest1
{
    public Mock<ILogger<ServerController>> _mockLogger = new();
    public ServerController _sut;

    public UnitTest1()
    {
        var context = new DefaultHttpContext();
        context.Connection.LocalPort = 9001;
        _sut = new ServerController(_mockLogger.Object);
        _sut.ControllerContext = new ControllerContext()
        {
            HttpContext = context
        };
        

    }

    [Fact]
    public void Connect_HasPort_OKReturned()
    {
        //Arrange


        //Act
        var result = _sut.Connect();


        //Assert
        _mockLogger.VerifyAll();
        result.Should().NotBe(null);
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public void Status_HasPort_OKReturnedWithConnectionInformation()
    {
        //Arrange


        //Act
        var result = _sut.Status();


        //Assert
        _mockLogger.VerifyAll();
        result.Should().NotBe(null);
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
