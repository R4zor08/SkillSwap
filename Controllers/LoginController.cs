// LoginController.cs
// Handles user authentication including login, registration, and logout
// Uses session-based authentication to maintain user state across requests

using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;
using SkillSwap.Web.ViewModels;

namespace SkillSwap.Web.Controllers
{
    /// <summary>
    /// Controller for managing user authentication (login, registration, logout)
    /// </summary>
    public class LoginController : Controller
    {
        // Service for student data operations
        private readonly IStudentService _studentService;
        // Service for authentication state management
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor with dependency injection of required services
        /// </summary>
        public LoginController(IStudentService studentService, IAuthService authService)
        {
            _studentService = studentService;
            _authService = authService;
        }

        /// <summary>
        /// GET: /Login/Index
        /// Displays the login page
        /// Redirects to Dashboard if user is already logged in
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            // Check if user is already logged in via session
            if (HttpContext.Session.GetString("StudentId") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        /// <summary>
        /// POST: /Login/Index
        /// Processes login form submission
        /// Validates credentials and creates session on success
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find student by email
                var student = _studentService.GetStudentByEmail(model.Email);
                if (student != null)
                {
                    // Verify password using BCrypt (secure comparison)
                    // BCrypt.Verify hashes the input and compares with stored hash
                    if (BCrypt.Net.BCrypt.Verify(model.Password, student.Password))
                    {
                        // Store user session data for authentication
                        _authService.Login(student);
                        
                        // Store user info in session for web access
                        HttpContext.Session.SetString("StudentId", student.StudentId.ToString());
                        HttpContext.Session.SetString("StudentName", student.Name);
                        HttpContext.Session.SetString("StudentEmail", student.Email);

                        // Redirect to Dashboard on successful login
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            return View(model);
        }

        /// <summary>
        /// GET: /Login/Register
        /// Displays the registration page for new users
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST: /Login/Register
        /// Processes registration form submission
        /// Creates new account with hashed password
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents CSRF attacks
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if email already exists (must be unique)
                    var existingStudent = _studentService.GetStudentByEmail(model.Email);
                    if (existingStudent != null)
                    {
                        ModelState.AddModelError("Email", "Email already registered.");
                        return View(model);
                    }

                    // Create new student with BCrypt hashed password
                    var student = _studentService.CreateStudent(model.Name, model.Email, model.Password);
                    
                    // Set success message and redirect to login
                    TempData["SuccessMessage"] = "Account created successfully. Please log in.";
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating student: {ex.Message}");
                }
            }

            return View(model);
        }

        /// <summary>
        /// POST: /Login/Logout
        /// Clears session and logs the user out
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.Logout();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
