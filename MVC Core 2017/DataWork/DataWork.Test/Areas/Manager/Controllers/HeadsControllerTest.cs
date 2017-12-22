namespace DataWork.Test.Areas.Manager.Controllers
{
    using DataWork.Data;
    using DataWork.Data.Models;
    using DataWork.Services.Implementations;
    using DataWork.Web;
    using DataWork.Web.Areas.Manager.ViewModels.Heads;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Areas.Manager.Controllers;
    using Xunit;

    public class HeadsControllerTest
    {

        [Fact]
        public void HeadsControllerShoudBeForInManagerArea()
        {
            //Arrange
            var controller = typeof(HeadsController);

            //Act
            var areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute)) as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(GlobalConstString.ManagerAreaName);

        }

        [Fact]
        public void HeadsControllerShoudBeForAuthorizeUser()
        {
            //Arrange
            var controller = typeof(HeadsController);

            //Act
            var attributes = controller
                .GetCustomAttributes(true);


            //Assert
            attributes
                .Should()
                .Match(at => at.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public void HeadsControllerShoudBeInRoleManager()
        {
            //Arrange
            var controller = typeof(HeadsController);

            //Act
            var attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;


            //Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be(GlobalConstString.ManagerRole);

        }

        [Fact]
        public async Task HeadsControllerIndexShoudBeReturnValidViewModel()
        {
            //Arrange
            var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var dbOptions = new DbContextOptionsBuilder<DataWorkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var db = new DataWorkDbContext(dbOptions);
            var leaveService = new LeaveService(db);

            var controller = new HeadsController(leaveService, userManager.Object);

            //Act
            var data = await leaveService.GetLeaveCountForApprove();
            var userAuthorize = await userManager.Object.IsInRoleAsync(Mock.Of<User>(), GlobalConstString.ManagerRole);

            var result = await controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<ViewResult>();

            result.As<ViewResult>()
                .Model
                .Should()
                .BeOfType<HeadsLeaveForApproveViewModel>();

            data
                .Should()
                .BeGreaterOrEqualTo(0);


        }

    }
}
