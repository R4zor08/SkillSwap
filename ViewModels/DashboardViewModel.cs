using SkillSwap.Models;

namespace SkillSwap.Web.ViewModels
{
    public class DashboardViewModel
    {
        public Student Student { get; set; } = null!;
        public List<Talent> Talents { get; set; } = new();
        public List<TradeRequest> IncomingRequests { get; set; } = new();
        public List<TradeRequest> OutgoingRequests { get; set; } = new();
    }
}
