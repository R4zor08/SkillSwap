using Microsoft.AspNetCore.Mvc;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Responses;

namespace SkillSwap.Web.Requests
{
    /// <summary>
    /// Base controller that provides Request pattern support
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected readonly IRequestService RequestService;
        
        protected BaseController(IRequestService requestService)
        {
            RequestService = requestService;
        }
        
        // Authentication helpers
        protected bool IsAuthenticated => RequestService.IsAuthenticated;
        protected Guid? CurrentUserId => RequestService.CurrentUserId != null 
            ? Guid.Parse(RequestService.CurrentUserId) 
            : null;
        protected string? CurrentUserName => RequestService.CurrentUserName;
        
        // Request data helpers
        protected string? Form(string key) => RequestService.Form(key);
        protected string? Query(string key) => RequestService.Query(key);
        protected Guid? QueryGuid(string key) => RequestService.QueryGuid(key);
        protected int? QueryInt(string key) => RequestService.QueryInt(key);
        
        // Session helpers
        protected void SetSession(string key, string value) => RequestService.SetSession(key, value);
        protected string? GetSession(string key) => RequestService.GetSession(key);
        
        // Response helpers
        protected IActionResult Success(string message = "Operation completed successfully")
        {
            return Json(new BaseResponse { Success = true, Message = message });
        }
        
        protected IActionResult Success<T>(T data, string message = "Operation completed successfully")
        {
            return Json(Response<T>.Ok(data, message));
        }
        
        protected IActionResult Failure(string message = "Operation failed")
        {
            return Json(BaseResponse.Fail(message));
        }
        
        protected IActionResult HandleResponse(BaseResponse response)
        {
            return Json(response);
        }
        
        protected IActionResult HandleResponse<T>(Response<T> response)
        {
            return Json(response);
        }
        
        // Redirect helpers
        protected IActionResult RequireAuthentication()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}
