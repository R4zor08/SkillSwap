using System;

namespace SkillSwap.Models
{
    public class Talent
    {
        private Guid _talentId;
        private string _talentName;
        private string _description;
        private Guid _studentId;
        private int _proficiencyLevel;

        public Talent(string talentName, string description, Guid studentId, int proficiencyLevel = 1)
        {
            _talentId = Guid.NewGuid();
            _talentName = talentName;
            _description = description;
            _studentId = studentId;
            _proficiencyLevel = proficiencyLevel;
        }

        // used by DataStore when hydrating from persistence
        internal Talent(Guid id, string talentName, string description, Guid studentId, int proficiencyLevel = 1)
        {
            _talentId = id;
            _talentName = talentName;
            _description = description;
            _studentId = studentId;
            _proficiencyLevel = proficiencyLevel;
        }

        public Guid TalentId => _talentId;

        public string TalentName
        {
            get => _talentName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Talent name cannot be empty");
                _talentName = value;
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public Guid StudentId => _studentId;

        public int ProficiencyLevel
        {
            get => _proficiencyLevel;
            set => _proficiencyLevel = value;
        }

        public override string ToString() => $"{TalentName} - {Description}";
    }
}
