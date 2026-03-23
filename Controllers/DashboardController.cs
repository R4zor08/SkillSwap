// DashboardController.cs
// Displays the user dashboard with personalized information
// Protected by [SessionAuthorize] attribute - only accessible to logged-in users

using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Web.ViewModels;
using SkillSwap.Web.Filters;

namespace SkillSwap.Web.Controllers
{
    /// <summary>
    /// Controller for the main user dashboard
    /// Requires authentication via SessionAuthorize filter
    /// </summary>
    [SessionAuthorize] // Custom authorization filter checks for valid session
    public class DashboardController : Controller
    {
        // Service dependencies injected via constructor
        private readonly IStudentService _studentService;    // Student data operations
        private readonly ITalentService _talentService;      // Talent/skill operations
        private readonly ITradeService _tradeService;      // Trade request operations

        /// <summary>
        /// Constructor with dependency injection of required services
        /// </summary>
        public DashboardController(IStudentService studentService, ITalentService talentService, ITradeService tradeService)
        {
            _studentService = studentService;
            _talentService = talentService;
            _tradeService = tradeService;
        }

        /// <summary>
        /// GET: /Dashboard/Index
        /// Displays the user's personalized dashboard
        /// Shows: student info, talents, incoming/outgoing trade requests
        /// Redirects to login if user is not authenticated
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            // Get the logged-in user's ID from session
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                // No session = not logged in, redirect to login
                return RedirectToAction("Index", "Login");
            }

            // Parse the student ID from session string
            var studentId = Guid.Parse(studentIdString);
            
            // Load the student from database
            var student = _studentService.GetStudentById(studentId);
            if (student == null)
            {
                // Student not found (deleted?), clear session and redirect
                return RedirectToAction("Index", "Login");
            }

            // Load user's talents from database
            var studentTalents = _talentService.GetTalentsByStudent(studentId);
            
            // Load trade requests where user is the target (incoming)
            var incomingRequests = _tradeService.GetIncomingTradeRequests(studentId);
            
            // Load trade requests where user is the requester (outgoing)
            var outgoingRequests = _tradeService.GetOutgoingTradeRequests(studentId);

            // Build the view model with all dashboard data
            var viewModel = new DashboardViewModel
            {
                Student = student,
                Talents = studentTalents.ToList(),
                IncomingRequests = incomingRequests.ToList(),
                OutgoingRequests = outgoingRequests.ToList()
            };

            return View(viewModel);
        }
    }
}
