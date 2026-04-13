using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SkillSwap.Data;
using SkillSwap.Data.Entities;
using SkillSwap.Models;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Services
{
    public class TalentService : ITalentService
    {
        private readonly ApplicationDbContext _context;

        public TalentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Talent> GetTalents()
        {
            return _context.Talents
                .AsNoTracking()
                .ToList()
                .Select(e => new Talent(e.TalentId, e.TalentName, e.Description, e.StudentId, e.ProficiencyLevel));
        }

        public Talent? GetTalentById(Guid id)
        {
            var e = _context.Talents.Find(id);
            return e == null ? null : new Talent(e.TalentId, e.TalentName, e.Description, e.StudentId, e.ProficiencyLevel);
        }

        public Talent AddTalent(string name, string description, Guid studentId, int proficiencyLevel = 1)
        {
            if (proficiencyLevel < 1) proficiencyLevel = 1;
            if (proficiencyLevel > 10) proficiencyLevel = 10;

            var entity = new TalentEntity { 
                TalentId = Guid.NewGuid(), 
                TalentName = name, 
                Description = description, 
                StudentId = studentId,
                ProficiencyLevel = proficiencyLevel
            };
            _context.Talents.Add(entity);
            _context.SaveChanges();
            return new Talent(entity.TalentId, entity.TalentName, entity.Description, entity.StudentId, entity.ProficiencyLevel);
        }

        public void UpdateTalent(Guid id, string name, string description, int proficiencyLevel)
        {
            var entity = _context.Talents.Find(id);
            if (entity == null) return;
            entity.TalentName = name;
            entity.Description = description;
            entity.ProficiencyLevel = proficiencyLevel;
            _context.SaveChanges();
        }

        public void DeleteTalent(Guid id)
        {
            var entity = _context.Talents.Find(id);
            if (entity == null) return;
            _context.Talents.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Talent> GetTalentsByStudent(Guid studentId)
        {
            return _context.Talents
                .AsNoTracking()
                .Where(t => t.StudentId == studentId)
                .ToList()
                .Select(e => new Talent(e.TalentId, e.TalentName, e.Description, e.StudentId, e.ProficiencyLevel));
        }

        public IEnumerable<Talent> GetAvailableTalentsForTrade(Guid studentId)
        {
            return _context.Talents
                .AsNoTracking()
                .Where(t => t.StudentId != studentId)
                .ToList()
                .Select(e => new Talent(e.TalentId, e.TalentName, e.Description, e.StudentId, e.ProficiencyLevel));
        }
    }
}
