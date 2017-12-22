namespace DataWork.Test.Areas.Administrator.Controllers
{
    using DataWork.Web;
    using DataWork.Web.Areas.Administrator.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    public class AdminControllerTest
    {
        [Fact]
        public void AdminControllerShoudBeForInAdministratorArea()
        {
            //Arrange
            var controller = typeof(AdminController);

            //Act
            var areaAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute)) as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(GlobalConstString.AdministratorAreaName);

        }

        [Fact]
        public void AdminControllerShoudBeForAuthorizeUser()
        {
            //Arrange
            var controller = typeof(AdminController);

            //Act
            var attributes = controller
                .GetCustomAttributes(true);


            //Assert
            attributes
                .Should()
                .Match(at => at.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public void AdminControllerShoudBeInRoleAdmin()
        {
            //Arrange
            var controller = typeof(AdminController);

            //Act
            var attribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;


            //Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be(GlobalConstString.AdministratorRole);

        }
    }
}
