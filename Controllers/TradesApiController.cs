using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;
using SkillSwap.Models;

namespace SkillSwap.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesApiController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradesApiController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // GET: api/TradesApi
        [HttpGet]
        public IActionResult GetAllTrades()
        {
            var trades = _tradeService.GetTrades();
            return Ok(trades);
        }

        // GET: api/TradesApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetTradeById(Guid id)
        {
            var trade = _tradeService.GetTradeById(id);
            if (trade == null)
            {
                return NotFound();
            }
            return Ok(trade);
        }

        // GET: api/TradesApi/incoming/{studentId}
        [HttpGet("incoming/{studentId}")]
        public IActionResult GetIncomingTrades(Guid studentId)
        {
            var trades = _tradeService.GetIncomingTradeRequests(studentId);
            return Ok(trades);
        }

        // GET: api/TradesApi/outgoing/{studentId}
        [HttpGet("outgoing/{studentId}")]
        public IActionResult GetOutgoingTrades(Guid studentId)
        {
            var trades = _tradeService.GetOutgoingTradeRequests(studentId);
            return Ok(trades);
        }

        // POST: api/TradesApi
        [HttpPost]
        public IActionResult CreateTrade([FromBody] CreateTradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trade = _tradeService.CreateTradeRequest(request.RequesterId, request.RequestedTalentId, request.OfferedTalentId, request.Message);
            return CreatedAtAction(nameof(GetTradeById), new { id = trade.TradeId }, trade);
        }

        // PUT: api/TradesApi/{id}/accept
        [HttpPut("{id}/accept")]
        public IActionResult AcceptTrade(Guid id)
        {
            var trade = _tradeService.GetTradeById(id);
            if (trade == null)
            {
                return NotFound();
            }

            _tradeService.AcceptTradeRequest(id);
            return NoContent();
        }

        // PUT: api/TradesApi/{id}/reject
        [HttpPut("{id}/reject")]
        public IActionResult RejectTrade(Guid id)
        {
            var trade = _tradeService.GetTradeById(id);
            if (trade == null)
            {
                return NotFound();
            }

            _tradeService.RejectTradeRequest(id);
            return NoContent();
        }

        // PUT: api/TradesApi/{id}/complete
        [HttpPut("{id}/complete")]
        public IActionResult CompleteTrade(Guid id, [FromBody] CompleteTradeRequest request)
        {
            var trade = _tradeService.GetTradeById(id);
            if (trade == null)
            {
                return NotFound();
            }

            _tradeService.CompleteTradeRequest(id);
            if (request.Rating.HasValue)
            {
                _tradeService.CompleteTrade(id, request.Rating.Value);
            }
            return NoContent();
        }
    }

    public class CreateTradeRequest
    {
        public Guid RequesterId { get; set; }
        public Guid RequestedTalentId { get; set; }
        public Guid OfferedTalentId { get; set; }
        public string Message { get; set; }
    }

    public class CompleteTradeRequest
    {
        public int? Rating { get; set; }
    }
}