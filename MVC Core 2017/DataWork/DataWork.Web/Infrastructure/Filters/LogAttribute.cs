namespace DataWork.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.IO;
    
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            using (StreamWriter writer = new StreamWriter("logs.txt", true))
            {
                //{date and time} – {IP Address} – {User/Anonymous} – {Controller}.{action}

                DateTime date = DateTime.UtcNow;
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
                string userName = context.HttpContext.User.Identity.IsAuthenticated ? context.HttpContext.User.Identity.Name : "Anonymous";
                string controllerName = context.RouteData.Values["controller"].ToString();
                string actionName = context.RouteData.Values["action"].ToString();

                string logMessage = $"{date} – {ipAddress} – {userName} – {controllerName}.{actionName}";

                if (context.ExceptionHandled)
                {
                    string exceptionType = context.Exception.GetType().Name;
                    string exceptionMessage = context.Exception.Message;

                    logMessage = $"[!] {logMessage} - {exceptionType} – {exceptionMessage}";
                }

                writer.WriteLine(logMessage);
            }

        }
    }
}
