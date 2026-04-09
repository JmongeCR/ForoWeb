using System;
using System.Linq;
using System.Web.Mvc;

namespace AP.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session == null || session["UserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Access" },
                        { "action", "Login" }
                    });
                return;
            }

            if (!string.IsNullOrWhiteSpace(Roles))
            {
                var currentRole = session["UserRole"]?.ToString() ?? string.Empty;
                var allowedRoles = Roles
                    .Split(',')
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToList();

                if (!allowedRoles.Any(r => r.Equals(currentRole, StringComparison.OrdinalIgnoreCase)))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" }
                        });
                }
            }
        }
    }
}
