using System;
using System.Collections.Generic;

namespace SkillSwap.Data.Entities
{
    public class TalentEntity
    {
        public Guid TalentId { get; set; }
        public string TalentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public int ProficiencyLevel { get; set; } = 1;

        public StudentEntity? Owner { get; set; }
        public List<TradeRequestEntity> TradeRequests { get; set; } = new();
    }
}
