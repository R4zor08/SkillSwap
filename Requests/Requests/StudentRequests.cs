using SkillSwap.Web.Requests.Requests;

namespace SkillSwap.Web.Requests.Requests
{
    /// <summary>
    /// Request for creating a new student
    /// </summary>
    public class CreateStudentRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
        protected override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Name is required");
            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required");
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                AddError(nameof(Password), "Password must be at least 6 characters");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for updating a student
    /// </summary>
    public class UpdateStudentRequest : BaseRequest
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        protected override bool Validate()
        {
            if (StudentId == Guid.Empty)
                AddError(nameof(StudentId), "Student ID is required");
            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Name is required");
            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for deleting a student
    /// </summary>
    public class DeleteStudentRequest : BaseRequest
    {
        public Guid StudentId { get; set; }
        
        protected override bool Validate()
        {
            if (StudentId == Guid.Empty)
                AddError(nameof(StudentId), "Student ID is required");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for login authentication
    /// </summary>
    public class LoginRequest : BaseRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        
        protected override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required");
            if (string.IsNullOrWhiteSpace(Password))
                AddError(nameof(Password), "Password is required");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for user registration
    /// </summary>
    public class RegisterRequest : BaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        
        protected override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Name is required");
            if (string.IsNullOrWhiteSpace(Email))
                AddError(nameof(Email), "Email is required");
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                AddError(nameof(Password), "Password must be at least 6 characters");
            if (Password != ConfirmPassword)
                AddError(nameof(ConfirmPassword), "Passwords do not match");
            return IsValid;
        }
    }
}
