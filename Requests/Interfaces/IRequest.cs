namespace SkillSwap.Web.Requests.Interfaces
{
    /// <summary>
    /// Base interface for all request DTOs
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Validates the request data
        /// </summary>
        bool IsValid { get; }
        
        /// <summary>
        /// Gets validation errors if any
        /// </summary>
        Dictionary<string, string> Errors { get; }
    }
}
