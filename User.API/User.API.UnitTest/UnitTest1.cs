using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace User.API.UnitTest
{
    public class UserControllerUnitTests
    {
        private Data.UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<Data.UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var userContext = new Data.UserContext(options);
            userContext.Users.Add(new Models.AppUser
            {
                Id = 1,
                Name = "testUser"
            });
            userContext.SaveChanges();
            return userContext;
        }
        private (Controllers.UserController controller, Data.UserContext usercontext) GetUserContriller()
        {
            var _context = GetUserContext();
            var loggerMoq = new Mock<ILogger<User.API.Controllers.UserController>>();
            var logger = loggerMoq.Object;
            //return (controller:new User.API.Controllers.UserController(_context, logger),usercontext: _context);

            return (controller: new User.API.Controllers.UserController(_context), usercontext: _context);
        }
        [Fact]
        public async Task GetReturnRightUser_WithExpectedParameters()
        {
           (Controllers.UserController controller, Data.UserContext usercontext) = GetUserContriller();
            var response = await controller.Get();
            //Assert.IsType<JsonResult>(response);
            var result=response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("testUser");
        }
        [Fact]
        public async Task Pathc_ReturnNewName_WithExpectdNewNameParameter()
        {
            (Controllers.UserController controller, Data.UserContext usercontext) = GetUserContriller();
            var document = new JsonPatchDocument<Models.AppUser>();
            document.Replace(user => user.Name, "updatedUser");
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            //assert response
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;
            appUser.Name.Should().Be("updatedUser");
            //assert name of value in context
            var userModel=await usercontext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("updatedUser");
        }
        [Fact]
        public async Task Pathc_ReturnNewProperties_WithExpectdAddProperties()
        {
            (Controllers.UserController controller, Data.UserContext usercontext) = GetUserContriller();
            var document = new JsonPatchDocument<Models.AppUser>();
            document.Replace(user => user.Properties, new List<Models.UserProperty> {
                new Models.UserProperty(){Key="test_key",Value="≤‚ ‘",Text="≤‚ ‘"}
            });
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            //assert response
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;
            appUser.Properties.Count.Should().Be(1);
            appUser.Properties.First().Value.Should().Be("≤‚ ‘");
            appUser.Properties.First().Key.Should().Be("test_key");
            //assert name of value in context
            var userModel = await usercontext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Count.Should().Be(1);
            userModel.Properties.First().Value.Should().Be("≤‚ ‘");
            userModel.Properties.First().Key.Should().Be("test_key");
        }
        [Fact]
        public async Task Pathc_ReturnNewProperties_WithExpectdRemoveProperty()
        {
            (Controllers.UserController controller, Data.UserContext usercontext) = GetUserContriller();
            var document = new JsonPatchDocument<Models.AppUser>();
            document.Replace(user => user.Properties, new List<Models.UserProperty> {
            });
            var response = await controller.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;
            //assert response
            var appUser = result.Value.Should().BeAssignableTo<Models.AppUser>().Subject;
            appUser.Properties.Should().BeEmpty();
            //assert name of value in context
            var userModel = await usercontext.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Properties.Should().BeEmpty();
        }
    }
}
