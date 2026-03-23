using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SkillSwap.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            var studentId = session.GetString("StudentId");

            if (string.IsNullOrEmpty(studentId))
            {
                // User is not logged in, redirect to login
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }
}
