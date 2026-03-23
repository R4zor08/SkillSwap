using SkillSwap.Web.Requests.Interfaces;

namespace SkillSwap.Web.Requests.Requests
{
    /// <summary>
    /// Base request class with validation support
    /// </summary>
    public abstract class BaseRequest : IRequest
    {
        private readonly Dictionary<string, string> _errors = new();
        
        public bool IsValid => _errors.Count == 0 && Validate();
        public Dictionary<string, string> Errors => _errors;
        
        /// <summary>
        /// Override to implement custom validation logic
        /// </summary>
        protected virtual bool Validate() => true;
        
        /// <summary>
        /// Adds a validation error
        /// </summary>
        protected void AddError(string field, string message)
        {
            _errors[field] = message;
        }
        
        /// <summary>
        /// Clears all validation errors
        /// </summary>
        protected void ClearErrors()
        {
            _errors.Clear();
        }
    }
}
