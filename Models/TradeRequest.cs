using System;

namespace SkillSwap.Models
{
    public enum TradeStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed
    }

    public class TradeRequest
    {
        private Guid _tradeId;
        private Guid _requesterId;
        private Guid _targetStudentId;
        private Guid _requestedTalentId;
        private Guid _offeredTalentId;
        private TradeStatus _status;
        private int? _rating;
        private string _message;

        public TradeRequest(Guid requesterId, Guid targetStudentId, Guid requestedTalentId, Guid offeredTalentId, string message = "")
        {
            _tradeId = Guid.NewGuid();
            _requesterId = requesterId;
            _targetStudentId = targetStudentId;
            _requestedTalentId = requestedTalentId;
            _offeredTalentId = offeredTalentId;
            _status = TradeStatus.Pending;
            _message = message ?? string.Empty;
            RequestedAt = DateTime.UtcNow;
        }

        // Legacy constructor for backward compatibility
        public TradeRequest(Guid requesterId, Guid talentId)
        {
            _tradeId = Guid.NewGuid();
            _requesterId = requesterId;
            _requestedTalentId = talentId;
            _status = TradeStatus.Pending;
            RequestedAt = DateTime.UtcNow;
        }

        // used by DataStore when hydrating from persistence
        internal TradeRequest(Guid tradeId, Guid requesterId, Guid targetStudentId, Guid requestedTalentId, Guid offeredTalentId, TradeStatus status, DateTime requestedAt, string message = "", int? rating = null)
        {
            _tradeId = tradeId;
            _requesterId = requesterId;
            _targetStudentId = targetStudentId;
            _requestedTalentId = requestedTalentId;
            _offeredTalentId = offeredTalentId;
            _status = status;
            _message = message ?? string.Empty;
            RequestedAt = requestedAt;
            _rating = rating;
        }

        // Legacy constructor for backward compatibility
        internal TradeRequest(Guid tradeId, Guid requesterId, Guid talentId, TradeStatus status, DateTime requestedAt, int? rating = null)
        {
            _tradeId = tradeId;
            _requesterId = requesterId;
            _requestedTalentId = talentId;
            _status = status;
            RequestedAt = requestedAt;
            _rating = rating;
        }

        public Guid TradeId => _tradeId;
        public Guid RequesterId => _requesterId;
        public Guid TargetStudentId => _targetStudentId;
        public Guid RequestedTalentId => _requestedTalentId;
        public Guid OfferedTalentId => _offeredTalentId;
        public TradeStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public DateTime RequestedAt { get; }

        public int? Rating
        {
            get => _rating;
            set => _rating = value;
        }

        public string Message
        {
            get => _message;
            set => _message = value;
        }

        // Navigation properties (for web interface)
        public Student? RequestingStudent { get; set; }
        public Student? TargetStudent { get; set; }
        public Talent? RequestedTalent { get; set; }
        public Talent? OfferedTalent { get; set; }

        public override string ToString() => $"Request {TradeId} - {Status} (Talent: {RequestedTalentId})";
    }
}
