using SkillSwap.Web.Requests.Requests;

namespace SkillSwap.Web.Requests.Requests
{
    /// <summary>
    /// Request for creating a trade request
    /// </summary>
    public class CreateTradeRequest : BaseRequest
    {
        public Guid RequestedTalentId { get; set; }
        public Guid OfferedTalentId { get; set; }
        public string Message { get; set; } = string.Empty;
        
        protected override bool Validate()
        {
            if (RequestedTalentId == Guid.Empty)
                AddError(nameof(RequestedTalentId), "Requested talent is required");
            if (OfferedTalentId == Guid.Empty)
                AddError(nameof(OfferedTalentId), "Offered talent is required");
            if (RequestedTalentId == OfferedTalentId)
                AddError(nameof(OfferedTalentId), "Cannot trade the same talent");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for updating trade status
    /// </summary>
    public class UpdateTradeStatusRequest : BaseRequest
    {
        public Guid TradeId { get; set; }
        public string Status { get; set; } = string.Empty; // "Accepted", "Rejected", "Completed"
        
        protected override bool Validate()
        {
            if (TradeId == Guid.Empty)
                AddError(nameof(TradeId), "Trade ID is required");
            var validStatuses = new[] { "Accepted", "Rejected", "Completed" };
            if (!validStatuses.Contains(Status))
                AddError(nameof(Status), "Invalid status. Must be Accepted, Rejected, or Completed");
            return IsValid;
        }
    }
}
