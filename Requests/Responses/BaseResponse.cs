using SkillSwap.Web.Requests.Interfaces;

namespace SkillSwap.Web.Requests.Responses
{
    /// <summary>
    /// Base response class with common properties
    /// </summary>
    public class BaseResponse : IResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        
        public static BaseResponse Ok(string message = "Operation completed successfully")
        {
            return new BaseResponse { Success = true, Message = message };
        }
        
        public static BaseResponse Fail(string message = "Operation failed")
        {
            return new BaseResponse { Success = false, Message = message };
        }
    }
    
    /// <summary>
    /// Generic response with data
    /// </summary>
    public class Response<T> : BaseResponse
    {
        public T? Data { get; set; }
        
        public static Response<T> Ok(T data, string message = "Operation completed successfully")
        {
            return new Response<T> { Success = true, Message = message, Data = data };
        }
        
        public new static Response<T> Fail(string message = "Operation failed")
        {
            return new Response<T> { Success = false, Message = message, Data = default };
        }
    }
}
