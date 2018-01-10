namespace DataWork.Test.Areas.Manager.Controllers
{
    using Data.Models;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using Web;
    using Web.Areas.Manager.Controllers;
    using Web.Areas.Manager.ViewModels.Heads;
    using Xunit;
    using DataWork.Test.Models;
    using System.Collections.Generic;
    using DataWork.Services.Models.Leave;
    using DataWork.Web.Controllers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
            var user = new Mock<User>();
            var userManager = this.CreateUserManager();
            var leaveService = new Mock<ILeaveService>();
            leaveService.Setup(x => x.GetLeaveCountForApprove())
                .ReturnsAsync(1);

            var controller = new HeadsController(leaveService.Object, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(GlobalConstString.ManagerRole);

            //Act
            var result = await controller.Index();
 
            //Assert
            result
                .Should()
                .BeOfType<ViewResult>();

            result.As<ViewResult>()
                .Model
                .Should()
                .BeOfType<HeadsLeaveForApproveViewModel>();

            var data = result
                        .As<ViewResult>()
                        .Model
                        .As<HeadsLeaveForApproveViewModel>();

            data
                .Count
                .Should()
                .BeGreaterOrEqualTo(0);


        }


        [Fact]
        public async Task HeadsControllerIndexShoudBeReturnValidRedirectIfUserNotInManagerRole()
        {
            //Arrange
            var user = new Mock<User>();
            var userManager = this.CreateUserManager();
            //var leaveService = new Mock<ILeaveService>();
            //leaveService.Setup(x => x.GetLeaveCountForApprove())
            //    .ReturnsAsync(1);

            var controller = new HeadsController(null, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty);

            //Act
            var result = await controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }

        [Fact]
        public async Task HeadsControllerIndexShoudBeReturnValidRedirectIfUserNotAuthorize()
        {
            //Arrange
            var controller = new HeadsController(null, null);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty, string.Empty);

            //Act
            var result = await controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }


        [Fact]
        public async Task HeadsControllerListShoudBeReturnValidRedirectIfUserNotInManagerRole()
        {
            //Arrange
            var user = new Mock<User>();
            var userManager = this.CreateUserManager();
            //var leaveService = new Mock<ILeaveService>();
            //leaveService.Setup(x => x.GetLeaveCountForApprove())
            //    .ReturnsAsync(1);

            var controller = new HeadsController(null, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty);

            //Act
            var result = await controller.List();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }

        [Fact]
        public async Task HeadsControllerListShoudBeReturnValidRedirectIfUserNotAuthorize()
        {
            //Arrange
            var controller = new HeadsController(null, null);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty, string.Empty);

            //Act
            var result = await controller.List();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }

        [Fact]
        public async Task HeadsControllerListShoudBeReturnValidViewModel()
        {
            //Arrange
            var user = new Mock<User>();
            var userManager = this.CreateUserManager();
            var leaveService = new Mock<ILeaveService>();
            leaveService.Setup(x => x.GetAllForApprove())
                .ReturnsAsync(new List<ListLeaveServiceModel>
                {
                    new ListLeaveServiceModel
                    {
                        CurrentYear = 2017,
                        FullName = "fullName2",
                        Id = 2,
                        LeaveStatus = LeaveStatus.Send

                    },

                    new ListLeaveServiceModel
                    {
                        CurrentYear = 2017,
                        FullName = "fullName1",
                        Id = 1,
                        LeaveStatus = LeaveStatus.Send

                    },

                    new ListLeaveServiceModel
                    {
                        CurrentYear = 2017,
                        FullName = "fullName3",
                        Id = 3,
                        LeaveStatus = LeaveStatus.Send

                    }
                });

            var controller = new HeadsController(leaveService.Object, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(GlobalConstString.ManagerRole);

            //Act
            var result = await controller.List();

            //Assert
            result
                .Should()
                .BeOfType<ViewResult>();

            result.As<ViewResult>()
                .Model
                .Should()
                .BeOfType<HeadsListLeaveViewModel>();

            var data = result
                        .As<ViewResult>()
                        .Model
                        .As<HeadsListLeaveViewModel>();

            data
                .LeavesList
                .Should()
                .BeOfType<List<ListLeaveServiceModel>>();

            data
                .LeavesList
                .Should()
                .Match(l => l.As<List<ListLeaveServiceModel>>().Count() == 3);

            var firstItem = data
                            .LeavesList
                            .First();

            firstItem.Should().Match(l => l.As<ListLeaveServiceModel>().Id == 2);
            firstItem.Should().Match(l => l.As<ListLeaveServiceModel>().FullName == "fullName2");
            firstItem.Should().Match(l => l.As<ListLeaveServiceModel>().LeaveStatus == LeaveStatus.Send);
            firstItem.Should().Match(l => l.As<ListLeaveServiceModel>().CurrentYear == 2017);

            var firstOrderItem = data
                .LeavesList
                .OrderByDescending(x => x.Id)
                .First();

            firstOrderItem.Should().Match(l => l.As<ListLeaveServiceModel>().Id == 3);
            firstOrderItem.Should().Match(l => l.As<ListLeaveServiceModel>().FullName == "fullName3");
            firstOrderItem.Should().Match(l => l.As<ListLeaveServiceModel>().LeaveStatus == LeaveStatus.Send);
            firstOrderItem.Should().Match(l => l.As<ListLeaveServiceModel>().CurrentYear == 2017);
        }


        [Fact]
        public async Task HeadsControllerChangeShoudBeValidRedirectIfIdOrLeaveStatusIsNotValid()
        {
            string errorMessage = null;

            //Arrange
            var userManager = this.CreateUserManager();
            var leaveService = new Mock<ILeaveService>();

            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(x => x[GlobalConstString.Error] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            var controller = new HeadsController(leaveService.Object, userManager.Object);
            controller.TempData = tempData.Object;

            //Act
            var result = await controller.Change(-1, LeaveStatus.Send);
            

            //Assert

            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(HeadsController.List));

            errorMessage.Should().Be(GlobalConstString.NotFound);

        }

        [Fact]
        public async Task HeadsControllerChangeShoudBeReturnValidRedirectIfUserNotInManagerRole()
        {
            //Arrange
            var user = new Mock<User>();
            var userManager = this.CreateUserManager();

            var controller = new HeadsController(null, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty);

            //Act
            var result = await controller.Change(1, LeaveStatus.Approve);

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }

        [Fact]
        public async Task HeadsControllerChangeShoudBeReturnValidRedirectIfUserNotAuthorize()
        {
            //Arrange
            var controller = new HeadsController(null, null);
            controller.ControllerContext = ControllerContextModel.Create(string.Empty, string.Empty);

            //Act
            var result = await controller.Change(1, LeaveStatus.Approve);

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Login");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Account");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(string.Empty);

        }

        [Fact]
        public async Task HeadsControllerChangeShoudBeValidRedirectIfLeaveNotExist()
        {
            string errorMessage = null;

            //Arrange
            var userManager = this.CreateUserManager();
            var leaveService = new Mock<ILeaveService>();
            leaveService
                .Setup(x => x.IsExist(It.IsAny<int>(), LeaveStatus.Create))
                .ReturnsAsync(false);


            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(x => x[GlobalConstString.Error] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);

            var controller = new HeadsController(leaveService.Object, userManager.Object);
            controller.ControllerContext = ControllerContextModel.Create(GlobalConstString.ManagerRole);
            controller.TempData = tempData.Object;

            //Act
            var result = await controller.Change(1, LeaveStatus.Approve);


            //Assert

            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(HeadsController.List));

            errorMessage.Should().Be(GlobalConstString.NotFound);

        }

        private Mock<UserManager<User>> CreateUserManager()
        {
            return new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        }

    }
}
