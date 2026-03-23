namespace SkillSwap.Web.Requests.Interfaces
{
    /// <summary>
    /// Base interface for all response DTOs
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        bool Success { get; set; }
        
        /// <summary>
        /// Message describing the result
        /// </summary>
        string Message { get; set; }
    }
}
