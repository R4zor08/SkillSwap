using SkillSwap.Web.Requests.Interfaces;
using Microsoft.AspNetCore.Http;

namespace SkillSwap.Web.Requests
{
    /// <summary>
    /// Service that provides centralized access to all HTTP request data
    /// </summary>
    public class RequestService : IRequestService
    {
        private readonly RequestContext _context;
        
        public RequestService(IHttpContextAccessor httpContextAccessor)
        {
            _context = new RequestContext(httpContextAccessor);
        }
        
        // Context Access
        public RequestContext Context => _context;
        public HttpContext? HttpContext => _context.HttpContext;
        public ISession? Session => _context.Session;
        
        // Authentication State
        public bool IsAuthenticated => _context.IsAuthenticated;
        public string? CurrentUserId => _context.CurrentUserId;
        public string? CurrentUserName => _context.CurrentUserName;
        public string? CurrentUserEmail => _context.CurrentUserEmail;
        
        // Request Info
        public string Method => _context.Method;
        public string Path => _context.Path;
        public bool IsPost => Method == "POST";
        public bool IsGet => Method == "GET";
        
        // Query Parameters
        public string? Query(string key) => _context.GetQueryParam(key);
        public T? Query<T>(string key) where T : struct => _context.GetQueryParam<T>(key);
        public Guid? QueryGuid(string key) => Query<Guid>(key);
        public int? QueryInt(string key) => Query<int>(key);
        
        // Form Data
        public string? Form(string key) => _context.GetFormField(key);
        public IFormFile? File(string key) => _context.GetFormFile(key);
        
        // Headers
        public string? Header(string key) => _context.GetHeader(key);
        
        // Session Management
        public void SetSession(string key, string value) => _context.SetSessionString(key, value);
        public string? GetSession(string key) => _context.GetSessionString(key);
        public void RemoveSession(string key) => _context.RemoveSessionKey(key);
        public void ClearSession() => _context.ClearSession();
        
        // Authentication Helpers
        public void LoginUser(Guid studentId, string name, string email)
        {
            SetSession("StudentId", studentId.ToString());
            SetSession("StudentName", name);
            SetSession("StudentEmail", email);
        }
        
        public void LogoutUser()
        {
            ClearSession();
        }
        
        // Request Binding
        public T? Bind<T>() where T : class, new()
        {
            var model = new T();
            var properties = typeof(T).GetProperties();
            
            foreach (var prop in properties)
            {
                var value = Form(prop.Name) ?? Query(prop.Name);
                if (value != null)
                {
                    try
                    {
                        var converted = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(model, converted);
                    }
                    catch
                    {
                        // Skip conversion errors
                    }
                }
            }
            
            return model;
        }
    }
}
