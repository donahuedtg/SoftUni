namespace DataWork.Test.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using Web;
    using Web.Controllers;
    using Xunit;

    public class LeavesControllerTest
    {
        [Fact]
        public void LeavesControllerShoudBeForAuthorizeUser()
        {
            //Arrange
            var controller = typeof(LeavesController);

            //Act
            var attributes = controller
                .GetCustomAttributes(false);


            //Assert
            attributes
                .Should()
                .Match(at => at.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public void LeavesControllerShoudBeInRoleWorker()
        {
            //Arrange
            var controller = typeof(LeavesController);

            //Act
            var attribute = controller
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;


            //Assert
            attribute.Should().NotBeNull();
            attribute.Roles.Should().Be(GlobalConstString.WorkerRole);

        }
    }
}
