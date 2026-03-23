using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;
using SkillSwap.Web.Filters;

namespace SkillSwap.Web.Controllers
{
    [SessionAuthorize]
    public class TalentsController : Controller
    {
        private readonly ITalentService _talentService;
        private readonly IStudentService _studentService;

        public TalentsController(ITalentService talentService, IStudentService studentService)
        {
            _talentService = talentService;
            _studentService = studentService;
        }

        // GET: Talents
        public IActionResult Index()
        {
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = Guid.Parse(studentIdString);
            var talents = _talentService.GetTalentsByStudent(studentId);
            return View(talents);
        }

        // GET: Talents/Create
        public IActionResult Create()
        {
            ViewBag.Students = _studentService.GetAllStudents();
            return View();
        }

        // POST: Talents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid studentId, string name, string description, int proficiencyLevel)
        {
            if (ModelState.IsValid)
            {
                _talentService.AddTalent(name, description, studentId);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _studentService.GetAllStudents();
            return View();
        }

        // GET: Talents/Edit/5
        public IActionResult Edit(Guid id)
        {
            var talent = _talentService.GetTalentById(id);
            if (talent == null)
            {
                return NotFound();
            }

            ViewBag.Students = _studentService.GetAllStudents();
            return View(talent);
        }

        // POST: Talents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, string name, string description, int proficiencyLevel)
        {
            if (ModelState.IsValid)
            {
                _talentService.UpdateTalent(id, name, description, proficiencyLevel);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _studentService.GetAllStudents();
            return View();
        }

        // GET: Talents/Delete/5
        public IActionResult Delete(Guid id)
        {
            var talent = _talentService.GetTalentById(id);
            if (talent == null)
            {
                return NotFound();
            }
            return View(talent);
        }

        // POST: Talents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _talentService.DeleteTalent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
