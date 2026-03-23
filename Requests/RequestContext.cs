using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SkillSwap.Web.Requests
{
    /// <summary>
    /// Centralized request context that wraps all HTTP request data
    /// </summary>
    public class RequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public HttpContext? HttpContext => _httpContextAccessor.HttpContext;
        public HttpRequest? Request => HttpContext?.Request;
        public HttpResponse? Response => HttpContext?.Response;
        public ISession? Session => HttpContext?.Session;
        
        // Session data helpers
        public string? CurrentUserId => Session?.GetString("StudentId");
        public string? CurrentUserName => Session?.GetString("StudentName");
        public string? CurrentUserEmail => Session?.GetString("StudentEmail");
        public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentUserId);
        
        // Request data helpers
        public string Method => Request?.Method ?? string.Empty;
        public string Path => Request?.Path.Value ?? string.Empty;
        public string QueryString => Request?.QueryString.Value ?? string.Empty;
        public IHeaderDictionary Headers => Request?.Headers ?? new HeaderDictionary();
        public IQueryCollection Query => Request?.Query ?? new QueryCollection();
        public IFormCollection Form => Request?.Form ?? new FormCollection(new Dictionary<string, StringValues>());
        
        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        // Query parameter helpers
        public string? GetQueryParam(string key) => Query.TryGetValue(key, out var value) ? value.ToString() : null;
        public T? GetQueryParam<T>(string key) where T : struct
        {
            var value = GetQueryParam(key);
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                return (T?)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return null;
            }
        }
        
        // Form data helpers
        public string? GetFormField(string key) => Form.TryGetValue(key, out var value) ? value.ToString() : null;
        public IFormFile? GetFormFile(string key) => Form.Files.GetFile(key);
        
        // Header helpers
        public string? GetHeader(string key) => Headers.TryGetValue(key, out var value) ? value.ToString() : null;
        
        // Session helpers
        public void SetSessionString(string key, string value) => Session?.SetString(key, value);
        public string? GetSessionString(string key) => Session?.GetString(key);
        public void RemoveSessionKey(string key) => Session?.Remove(key);
        public void ClearSession() => Session?.Clear();
    }
}
