using System;
using System.Collections.Generic;
using SkillSwap.Models;

namespace SkillSwap.Services.Interfaces
{
    public interface ITalentService
    {
        IEnumerable<Talent> GetTalents();
        Talent? GetTalentById(Guid id);
        Talent AddTalent(string name, string description, Guid studentId, int proficiencyLevel = 1);
        void UpdateTalent(Guid id, string name, string description, int proficiencyLevel);
        void DeleteTalent(Guid id);
        IEnumerable<Talent> GetTalentsByStudent(Guid studentId);
        IEnumerable<Talent> GetAvailableTalentsForTrade(Guid studentId);
    }
}
