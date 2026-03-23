using SkillSwap.Web.Requests.Requests;

namespace SkillSwap.Web.Requests.Requests
{
    /// <summary>
    /// Request for creating a new talent
    /// </summary>
    public class CreateTalentRequest : BaseRequest
    {
        public Guid StudentId { get; set; }
        public string TalentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; } = 1;
        
        protected override bool Validate()
        {
            if (StudentId == Guid.Empty)
                AddError(nameof(StudentId), "Student ID is required");
            if (string.IsNullOrWhiteSpace(TalentName))
                AddError(nameof(TalentName), "Talent name is required");
            if (ProficiencyLevel < 1 || ProficiencyLevel > 5)
                AddError(nameof(ProficiencyLevel), "Proficiency level must be between 1 and 5");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for updating a talent
    /// </summary>
    public class UpdateTalentRequest : BaseRequest
    {
        public Guid TalentId { get; set; }
        public string TalentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProficiencyLevel { get; set; } = 1;
        
        protected override bool Validate()
        {
            if (TalentId == Guid.Empty)
                AddError(nameof(TalentId), "Talent ID is required");
            if (string.IsNullOrWhiteSpace(TalentName))
                AddError(nameof(TalentName), "Talent name is required");
            if (ProficiencyLevel < 1 || ProficiencyLevel > 5)
                AddError(nameof(ProficiencyLevel), "Proficiency level must be between 1 and 5");
            return IsValid;
        }
    }
    
    /// <summary>
    /// Request for deleting a talent
    /// </summary>
    public class DeleteTalentRequest : BaseRequest
    {
        public Guid TalentId { get; set; }
        
        protected override bool Validate()
        {
            if (TalentId == Guid.Empty)
                AddError(nameof(TalentId), "Talent ID is required");
            return IsValid;
        }
    }
}
