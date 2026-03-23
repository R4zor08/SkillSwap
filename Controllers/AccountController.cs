using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;

namespace SkillSwap.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IAuthService _authService;

        public AccountController(IStudentService studentService, IAuthService authService)
        {
            _studentService = studentService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var students = _studentService.GetAllStudents();
            ViewBag.Students = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(students, "StudentId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Login(Guid studentId)
        {
            var student = _studentService.GetStudentById(studentId);
            if (student != null)
            {
                // Store user in session
                HttpContext.Session.SetString("StudentId", student.StudentId.ToString());
                HttpContext.Session.SetString("StudentName", student.Name);
                
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid student selection");
            
            // Repopulate dropdown on error
            var students = _studentService.GetAllStudents();
            ViewBag.Students = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(students, "StudentId", "Name");
            
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string email)
        {
            if (ModelState.IsValid)
            {
                var student = _studentService.CreateStudent(name, email);
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}
