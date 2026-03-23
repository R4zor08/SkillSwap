using Microsoft.AspNetCore.Http;

namespace SkillSwap.Web.Requests.Interfaces
{
    /// <summary>
    /// Interface for the Request Service
    /// </summary>
    public interface IRequestService
    {
        RequestContext Context { get; }
        HttpContext? HttpContext { get; }
        ISession? Session { get; }
        
        bool IsAuthenticated { get; }
        string? CurrentUserId { get; }
        string? CurrentUserName { get; }
        string? CurrentUserEmail { get; }
        
        string Method { get; }
        string Path { get; }
        bool IsPost { get; }
        bool IsGet { get; }
        
        string? Query(string key);
        T? Query<T>(string key) where T : struct;
        Guid? QueryGuid(string key);
        int? QueryInt(string key);
        
        string? Form(string key);
        IFormFile? File(string key);
        
        string? Header(string key);
        
        void SetSession(string key, string value);
        string? GetSession(string key);
        void RemoveSession(string key);
        void ClearSession();
        
        void LoginUser(Guid studentId, string name, string email);
        void LogoutUser();
        
        T? Bind<T>() where T : class, new();
    }
}
