using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using webapi.Controllers;
using webapi.Models;
using webapi.Services;
using webapi.Services.Interfaces;

namespace LibraryProject.UnitTests.ControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public void GetUserBooks_Return_Ok()
        {
            //Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,"Name")
            }));
            var userServicesMock = new Mock<IUserService>();
            var userController = new UserController(userServicesMock.Object);
            userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            //Act
            var actionResult = userController.GetUserBooks();

            var result = actionResult as OkObjectResult;
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.NotNull(result.Value);

        }
        [Fact]
        public void AddBookToLibrary_Return_BadRequest_When_Fails()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,"Name")
            }));
            var userServicesMock = new Mock<IUserService>();
            var userController = new UserController(userServicesMock.Object);
            userServicesMock.Setup(x => x.AddBookToLibrary(It.IsAny<string>(), It.IsAny<string>())).Returns<Book>(null);
            userController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var actionResult = userController.AddBookToLibrary("NonExistedId");

            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}
