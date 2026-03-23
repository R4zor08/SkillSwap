using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;

namespace SkillSwap.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Students
        public IActionResult Index()
        {
            var students = _studentService.GetAllStudents();
            return View(students);
        }

        // GET: Students/Details/5
        public IActionResult Details(Guid id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name, string email)
        {
            if (ModelState.IsValid)
            {
                _studentService.CreateStudent(name, email, "password");
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Students/Edit/5
        public IActionResult Edit(Guid id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, string name, string email)
        {
            if (ModelState.IsValid)
            {
                _studentService.UpdateStudent(id, name, email);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Students/Delete/5
        public IActionResult Delete(Guid id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _studentService.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
