using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Requests;
using SkillSwap.Web.Requests.Responses;

namespace SkillSwap.Web.Requests.Handlers
{
    /// <summary>
    /// Handles all talent-related requests
    /// </summary>
    public class TalentRequestHandler : 
        IRequestHandler<CreateTalentRequest, Response<Guid>>,
        IRequestHandler<UpdateTalentRequest, BaseResponse>,
        IRequestHandler<DeleteTalentRequest, BaseResponse>
    {
        private readonly ITalentService _talentService;
        private readonly IRequestService _requestService;
        
        public TalentRequestHandler(ITalentService talentService, IRequestService requestService)
        {
            _talentService = talentService;
            _requestService = requestService;
        }
        
        public async Task<Response<Guid>> HandleAsync(CreateTalentRequest request)
        {
            if (!request.IsValid)
                return Response<Guid>.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                var talent = _talentService.AddTalent(
                    request.TalentName,
                    request.Description,
                    request.StudentId);
                return Response<Guid>.Ok(talent.TalentId, "Talent created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail($"Failed to create talent: {ex.Message}");
            }
        }
        
        public async Task<BaseResponse> HandleAsync(UpdateTalentRequest request)
        {
            if (!request.IsValid)
                return BaseResponse.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                _talentService.UpdateTalent(
                    request.TalentId, 
                    request.TalentName, 
                    request.Description, 
                    request.ProficiencyLevel);
                return BaseResponse.Ok("Talent updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.Fail($"Failed to update talent: {ex.Message}");
            }
        }
        
        public async Task<BaseResponse> HandleAsync(DeleteTalentRequest request)
        {
            if (!request.IsValid)
                return BaseResponse.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                _talentService.DeleteTalent(request.TalentId);
                return BaseResponse.Ok("Talent deleted successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.Fail($"Failed to delete talent: {ex.Message}");
            }
        }
    }
}
