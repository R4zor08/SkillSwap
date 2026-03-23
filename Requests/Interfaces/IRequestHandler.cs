namespace SkillSwap.Web.Requests.Interfaces
{
    /// <summary>
    /// Interface for request handlers that process specific request types
    /// </summary>
    /// <typeparam name="TRequest">The type of request to handle</typeparam>
    /// <typeparam name="TResponse">The type of response to return</typeparam>
    public interface IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest
        where TResponse : IResponse
    {
        /// <summary>
        /// Handles the request and returns a response
        /// </summary>
        Task<TResponse> HandleAsync(TRequest request);
    }
}
