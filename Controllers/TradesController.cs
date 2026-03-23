using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;
using SkillSwap.Web.ViewModels;
using SkillSwap.Web.Filters;

namespace SkillSwap.Web.Controllers
{
    [SessionAuthorize]
    public class TradesController : Controller
    {
        private readonly ITradeService _tradeService;
        private readonly IStudentService _studentService;
        private readonly ITalentService _talentService;

        public TradesController(ITradeService tradeService, IStudentService studentService, ITalentService talentService)
        {
            _tradeService = tradeService;
            _studentService = studentService;
            _talentService = talentService;
        }

        // GET: Trades
        public IActionResult Index()
        {
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = Guid.Parse(studentIdString);
            var incomingRequests = _tradeService.GetIncomingTradeRequests(studentId);
            var outgoingRequests = _tradeService.GetOutgoingTradeRequests(studentId);

            var viewModel = new TradeIndexViewModel
            {
                IncomingRequests = incomingRequests.ToList(),
                OutgoingRequests = outgoingRequests.ToList()
            };

            return View(viewModel);
        }

        // GET: Trades/Create
        public IActionResult Create()
        {
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = Guid.Parse(studentIdString);
            var availableTalents = _talentService.GetAvailableTalentsForTrade(studentId);
            
            ViewBag.AvailableTalents = availableTalents;
            ViewBag.MyTalents = _talentService.GetTalentsByStudent(studentId);
            
            return View();
        }

        // POST: Trades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid requestedTalentId, Guid offeredTalentId, string message)
        {
            var studentIdString = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var studentId = Guid.Parse(studentIdString);

            if (ModelState.IsValid)
            {
                _tradeService.CreateTradeRequest(studentId, requestedTalentId, offeredTalentId, message);
                return RedirectToAction(nameof(Index));
            }

            var availableTalents = _talentService.GetAvailableTalentsForTrade(studentId);
            ViewBag.AvailableTalents = availableTalents;
            ViewBag.MyTalents = _talentService.GetTalentsByStudent(studentId);
            
            return View();
        }

        // POST: Trades/Accept/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Accept(Guid id)
        {
            _tradeService.AcceptTradeRequest(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Trades/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(Guid id)
        {
            _tradeService.RejectTradeRequest(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Trades/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(Guid id)
        {
            _tradeService.CompleteTradeRequest(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Trades/Details/5
        public IActionResult Details(Guid id)
        {
            var tradeRequest = _tradeService.GetTradeRequestById(id);
            if (tradeRequest == null)
            {
                return NotFound();
            }
            return View(tradeRequest);
        }
    }
}
