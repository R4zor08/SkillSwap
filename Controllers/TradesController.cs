using System.Linq;
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
            return View(BuildTradeCreateViewModel(studentId));
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
            var vm = BuildTradeCreateViewModel(studentId);

            if (requestedTalentId == Guid.Empty || offeredTalentId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Select both the skill you want and the skill you offer.");
                return View(vm);
            }

            var offered = _talentService.GetTalentById(offeredTalentId);
            if (offered == null || offered.StudentId != studentId)
            {
                ModelState.AddModelError(string.Empty, "You must offer one of your own skills.");
                return View(vm);
            }

            var requested = _talentService.GetTalentById(requestedTalentId);
            if (requested == null || requested.StudentId == studentId)
            {
                ModelState.AddModelError(string.Empty, "Choose a skill listed by another student.");
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                _tradeService.CreateTradeRequest(studentId, requestedTalentId, offeredTalentId, message ?? string.Empty);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        private TradeCreateViewModel BuildTradeCreateViewModel(Guid studentId)
        {
            var availableTalents = _talentService.GetAvailableTalentsForTrade(studentId);
            var myTalents = _talentService.GetTalentsByStudent(studentId);

            return new TradeCreateViewModel
            {
                AvailableTalents = availableTalents.Select(t => new TradeTalentPickOption
                {
                    TalentId = t.TalentId,
                    TalentName = t.TalentName,
                    ProficiencyLevel = t.ProficiencyLevel,
                    OwnerName = _studentService.GetStudentById(t.StudentId)?.Name ?? "Unknown"
                }).ToList(),
                MyTalents = myTalents.Select(t => new TradeTalentPickOption
                {
                    TalentId = t.TalentId,
                    TalentName = t.TalentName,
                    ProficiencyLevel = t.ProficiencyLevel
                }).ToList()
            };
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
