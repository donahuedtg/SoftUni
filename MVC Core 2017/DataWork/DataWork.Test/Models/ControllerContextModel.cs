namespace DataWork.Test.Models
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public static class ControllerContextModel
    {
        public static ControllerContext Create(string role, string authType = "true")
        {
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                                    new Claim(ClaimTypes.Name, string.Empty),
                                    new Claim(ClaimTypes.Role, role)
                    }, authType)),
                }
            };
        }
    }
}
