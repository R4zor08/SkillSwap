// RegisterViewModel.cs
// ViewModel for the registration form
// Used to capture and validate new user registration information

using System.ComponentModel.DataAnnotations;

namespace SkillSwap.Web.ViewModels
{
    /// <summary>
    /// ViewModel for user registration form
    /// Contains name, email, password with validation rules
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// User's full name
        /// Required, max 100 characters
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// Required, must be valid email format, max 100 characters
        /// Must be unique across all users
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password
        /// Required, min 6 characters, max 100 characters
        /// Stored as BCrypt hash in database, never plain text
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password confirmation field
        /// Must match the Password field exactly
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
