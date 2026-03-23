// LoginViewModel.cs
// ViewModel for the login form
// Used to capture and validate user credentials during login

using System.ComponentModel.DataAnnotations;

namespace SkillSwap.Web.ViewModels
{
    /// <summary>
    /// ViewModel for user login form
    /// Contains email, password fields with validation attributes
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User's email address
        /// Required field with email format validation
        /// Must be a valid email address to proceed with login
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password
        /// Required field, displayed as password input (masked)
        /// Must match the password stored in the database for the given email
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Remember me option for persistent login
        /// Optional field, not required
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
