using System;
using System.Collections.Generic;

namespace SkillSwap.Web.ViewModels
{
    public class TradeCreateViewModel
    {
        public List<TradeTalentPickOption> AvailableTalents { get; set; } = new();
        public List<TradeTalentPickOption> MyTalents { get; set; } = new();
    }

    /// <summary>
    /// Options for trade create dropdowns (strongly typed for Razor; avoids dynamic ViewBag errors).
    /// </summary>
    public class TradeTalentPickOption
    {
        public Guid TalentId { get; set; }
        public string TalentName { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; }
        public string? OwnerName { get; set; }
    }
}
