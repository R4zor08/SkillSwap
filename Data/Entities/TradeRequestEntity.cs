using System;

namespace SkillSwap.Data.Entities
{
    public class TradeRequestEntity
    {
        public Guid TradeId { get; set; }
        public Guid RequesterId { get; set; }
        public Guid TargetStudentId { get; set; }
        public Guid RequestedTalentId { get; set; }
        public Guid OfferedTalentId { get; set; }
        public int Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public int? Rating { get; set; }
        public string Message { get; set; } = string.Empty;

        // Navigation properties
        public StudentEntity? Requester { get; set; }
        public StudentEntity? TargetStudent { get; set; }
        public TalentEntity? RequestedTalent { get; set; }
        public TalentEntity? OfferedTalent { get; set; }
    }
}
