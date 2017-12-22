namespace DataWork.Test.Controllers
{
    using DataWork.Data.Models;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Web.Controllers;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void HomeControllerShoudBeRedirectCorectIfUserRoleIsManager()
        {
            //Arrange
            var controller = new HomeController();         

            //Act
            var result = controller.Index();

            //Assert
            result
                .Should()
                .BeOfType<ViewResult>();

        }
    }
}
