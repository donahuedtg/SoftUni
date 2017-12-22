namespace DataWork.Test.Controllers
{
    using DataWork.Web;
    using DataWork.Web.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using Xunit;

    public class WorksControllerTest
    {
        [Fact]
        public void WorksControllerShoudBeForAuthorizeUser()
        {
            //Arrange
            var controller = typeof(WorksController);

            //Act
            var attributes = controller
                .GetCustomAttributes(false);


            //Assert
            attributes
                .Should()
                .Match(at => at.Any(a => a.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public void WorksControllerShoudBeInRoleWorker()
        {
            //Arrange
            var controller = typeof(WorksController);

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
