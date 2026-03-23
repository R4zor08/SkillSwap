using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;
using SkillSwap.Web.ViewModels;

namespace SkillSwap.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ITalentService _talentService;
        private readonly ITradeService _tradeService;

        public HomeController(IStudentService studentService, ITalentService talentService, ITradeService tradeService)
        {
            _studentService = studentService;
            _talentService = talentService;
            _tradeService = tradeService;
        }

        public IActionResult Index()
        {
            // Check if user is logged in
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                return RedirectToAction("Index", "Login");
            }

            var studentId = Guid.Parse(studentIdString);
            var student = _studentService.GetStudentById(studentId);
            
            if (student == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var studentTalents = _talentService.GetTalentsByStudent(studentId);
            var incomingRequests = _tradeService.GetIncomingTradeRequests(studentId);
            var outgoingRequests = _tradeService.GetOutgoingTradeRequests(studentId);

            var viewModel = new DashboardViewModel
            {
                Student = student,
                Talents = studentTalents.ToList(),
                IncomingRequests = incomingRequests.ToList(),
                OutgoingRequests = outgoingRequests.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
