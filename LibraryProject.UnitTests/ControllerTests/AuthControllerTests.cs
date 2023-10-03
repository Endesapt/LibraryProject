using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapi.Controllers;
using webapi.Dto;
using webapi.Models;
using webapi.Services.Interfaces;

namespace LibraryProject.UnitTests.ControllerTests
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("name","surname")]
        public async void Login_Returns_Ok_WithValid(string name, string password)
        {
            //Arrange
            AuthenticateDto authenticateDto = new AuthenticateDto()
            {
                Name=name,
                Password=password
            };
            
            var userServiceMock = new Mock<IUserService>();
            var user=new User() {
                Name=authenticateDto.Name
            };
            userServiceMock.Setup(mock => mock.Login(authenticateDto))
                .Returns(user);
            userServiceMock.Setup(mock => mock.CreateToken(user))
                .Returns("myToken");
            var userController = new AuthController(userServiceMock.Object);
            //Act
            var actionResult = userController.Login(authenticateDto);
            //Assert
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result.Value);
        }
        [Fact]
        public async void Login_Returns_BadRequest_WhenInvalid()
        {
            //Arrange
            AuthenticateDto authenticateDto = new AuthenticateDto()
            {
                Name ="",
                Password=""
            };

            var userServiceMock = new Mock<IUserService>();
            var user = new User()
            {
                Name = authenticateDto.Name
            };
            userServiceMock.Setup(mock => mock.Login(authenticateDto))
                .Returns(user);
            userServiceMock.Setup(mock => mock.CreateToken(user))
                .Returns("myToken");
            var userController = new AuthController(userServiceMock.Object);
            //Act
            var actionResult= userController.Login(authenticateDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Theory]
        [InlineData("name","password")]
        public async void Register_Returns_Ok_WhenValid(string name,string password)
        {
            //Arrange
            AuthenticateDto authenticateDto = new AuthenticateDto()
            {
                Name = name,
                Password = password
            };

            var userServiceMock = new Mock<IUserService>();
            var user = new User()
            {
                Name = authenticateDto.Name
            };
            userServiceMock.Setup(mock => mock.Register(authenticateDto))
                .Returns(Task.FromResult(user));
            userServiceMock.Setup(mock => mock.CreateToken(user))
                .Returns("myToken");
            var userController = new AuthController(userServiceMock.Object);
            //Act
            var actionResult = await userController.Register(authenticateDto);
            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result.Value);
        }
    }
}
