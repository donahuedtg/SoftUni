namespace DataWork.Test.Controllers
{
    using DataWork.Test.Models;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Web;
    using Web.Controllers;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void HomeControllerShoudBeRedirectCorectIfUserRoleIsManager()
        {
            //Arrange
            var controller = new HomeController();
            controller.ControllerContext = ControllerContextModel.Create(GlobalConstString.ManagerRole);

            //Act
            var result = controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Index");
            result.As<RedirectToActionResult>().ControllerName.Should().Be(GlobalConstString.ControllerHeads);
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(GlobalConstString.ManagerAreaName);
        }

        [Fact]
        public void HomeControllerShoudBeRedirectCorectIfUserRoleIsAdministrator()
        {
            //Arrange
            var controller = new HomeController();
            controller.ControllerContext = ControllerContextModel.Create(GlobalConstString.AdministratorRole);

            //Act
            var result = controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Index");
            result.As<RedirectToActionResult>().ControllerName.Should().Be(GlobalConstString.ControllerAdmin);
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
            result.As<RedirectToActionResult>().RouteValues.Values.Should().Contain(GlobalConstString.AdministratorAreaName);

        }

    }
}
