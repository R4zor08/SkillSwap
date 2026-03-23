using System;
using System.Collections.Generic;

namespace SkillSwap.Data.Entities
{
    public class StudentEntity
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<TalentEntity> Talents { get; set; } = new();
    }
}
