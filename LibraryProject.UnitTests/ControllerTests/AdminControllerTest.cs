using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapi.Controllers;
using webapi.Dto;
using webapi.Models;
using webapi.Services.Interfaces;
using Xunit;

namespace LibraryProject.UnitTests.ControllerTests
{
    public  class AdminControllerTest
    {
        [Fact]
        public void Login_Returns_OK_When_Valid()
        {
            AuthenticateDto authenticateDto = new AuthenticateDto()
            {
                Name = "Name",
                Password = "Password"
            };

            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock =new Mock<IBookService>();
            var admin = new Admin()
            {
                Name = authenticateDto.Name
            };
            adminServiceMock.Setup(mock => mock.Login(authenticateDto))
                .Returns(admin);
            adminServiceMock.Setup(mock => mock.CreateToken(admin))
                .Returns("myToken");
            var adminController = new AdminController(adminServiceMock.Object,bookServiceMock.Object);
            //Act
            var actionResult = adminController.Login(authenticateDto);
            //Assert
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result.Value);
        }
        [Fact]
        public void Login_Returns_BadResult_When_InValid()
        {
            AuthenticateDto authenticateDto = new AuthenticateDto()
            {
                Name = null,
                Password = "Password"
            };

            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock = new Mock<IBookService>();
            var admin = new Admin()
            {
                Name = authenticateDto.Name
            };
            adminServiceMock.Setup(mock => mock.Login(authenticateDto))
                .Returns(admin);
            adminServiceMock.Setup(mock => mock.CreateToken(admin))
                .Returns("myToken");
            var adminController = new AdminController(adminServiceMock.Object, bookServiceMock.Object);
            //Act
            var actionResult = adminController.Login(authenticateDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        [Fact]
        public async void CreateBook_Returns_Ok_WhenValid()
        {
            Book book=new Book() {
                Title = "Test",

            };

            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock = new Mock<IBookService>();
            var fileMock = new Mock<IFormFile>();
            var bookDto = new BookDto()
            {
                Title = "Test",
                Author="Test",
                Description="Test",
                File=fileMock.Object,
            };
            bookServiceMock.Setup(mock=>mock.CreateBook(bookDto,fileMock.Object))
                .Returns(Task.FromResult(book));
            var adminController = new AdminController(adminServiceMock.Object, bookServiceMock.Object);
            //Act
            var actionResult = await adminController.CreateBook(bookDto);
            //Assert
            var result = actionResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result.Value);

        }
        [Fact]
        public async void CreateBook_Returns_BadRequest_WhenInValid()
        {
            Book book = new Book()
            {
                Title = null,
            };

            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock = new Mock<IBookService>();
            var fileMock = new Mock<IFormFile>();
            var bookDto = new BookDto()
            {
                Title = "Test",
            };
            bookServiceMock.Setup(mock => mock.CreateBook(bookDto, fileMock.Object))
                .Returns(Task.FromResult(book));
            var adminController = new AdminController(adminServiceMock.Object, bookServiceMock.Object);
            //Act
            var actionResult = await adminController.CreateBook(bookDto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);

        }
        [Fact]
        public void DeleteBook_Return_Ok_WhenValid()
        {
            //Arrange
            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock = new Mock<IBookService>();
            var adminController = new AdminController(adminServiceMock.Object, bookServiceMock.Object);
            string id= Guid.NewGuid().ToString();
            bookServiceMock.Setup(mock => mock.DeleteBookById(id)).Returns(new Book() {});
            //Act
            var actionResult = adminController.DeleteBook(id);
            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }
        [Theory]
        [InlineData(null,"password")]
        public void CreateAdmin_Return_BadRequest_WhenInvalid(string name,string password)
        {
            //Arrange
            var adminServiceMock = new Mock<IAdminService>();
            var bookServiceMock = new Mock<IBookService>();
            var adminController = new AdminController(adminServiceMock.Object, bookServiceMock.Object);
            AuthenticateDto dto = new()
            {
                Name = name,
                Password = password
            };
            adminServiceMock.Setup(mock => mock.CreateAdmin(dto)).Returns(new Admin());
            
            //Act
            var actionResult=adminController.CreateAdmin(dto);
            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

    }
}
