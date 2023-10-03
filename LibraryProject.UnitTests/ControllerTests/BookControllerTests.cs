using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Controllers;
using webapi.Models;
using webapi.Services.Interfaces;

namespace LibraryProject.UnitTests.ControllerTests
{
    public class BookControllerTests
    {
        [Fact]
        public void GetAllBooks_Returns_Ok()
        {
            //Arrange
            var appEnvironment = new Mock<IWebHostEnvironment>().Object;
            var bookServices = new Mock<IBookService>();
            bookServices.Setup(mock => mock.GetAllBooks(It.IsAny<int>())).Returns(new List<Book>());
            var bookController = new BookController(bookServices.Object,appEnvironment);
            //Act
            var result=bookController.GetAllBooks() as OkObjectResult;
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Book>>(result.Value);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FetBookById_Returns_BadRequest_WhenInValid(string ?id) {
            //Arrange
            var appEnvironment = new Mock<IWebHostEnvironment>().Object;
            var bookServices = new Mock<IBookService>();
            bookServices.Setup(mock => mock.GetBookById(It.IsAny<string>())).Returns(new Book() {});
            var bookController = new BookController(bookServices.Object, appEnvironment);
            //Act
            var result = bookController.GetBookById(id);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
