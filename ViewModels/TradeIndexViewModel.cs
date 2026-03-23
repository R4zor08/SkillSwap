using SkillSwap.Models;

namespace SkillSwap.Web.ViewModels
{
    public class TradeIndexViewModel
    {
        public List<TradeRequest> IncomingRequests { get; set; } = new();
        public List<TradeRequest> OutgoingRequests { get; set; } = new();
    }
}
