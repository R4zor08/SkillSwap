using SkillSwap.Services.Interfaces;
using SkillSwap.Web.Requests.Interfaces;
using SkillSwap.Web.Requests.Requests;
using SkillSwap.Web.Requests.Responses;

namespace SkillSwap.Web.Requests.Handlers
{
    /// <summary>
    /// Handles all trade-related requests
    /// </summary>
    public class TradeRequestHandler : 
        IRequestHandler<CreateTradeRequest, Response<Guid>>,
        IRequestHandler<UpdateTradeStatusRequest, BaseResponse>
    {
        private readonly ITradeService _tradeService;
        private readonly IRequestService _requestService;
        
        public TradeRequestHandler(ITradeService tradeService, IRequestService requestService)
        {
            _tradeService = tradeService;
            _requestService = requestService;
        }
        
        public async Task<Response<Guid>> HandleAsync(CreateTradeRequest request)
        {
            if (!request.IsValid)
                return Response<Guid>.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                if (!_requestService.IsAuthenticated || string.IsNullOrWhiteSpace(_requestService.CurrentUserId))
                    return Response<Guid>.Fail("User is not authenticated");

                if (!Guid.TryParse(_requestService.CurrentUserId, out var requesterId))
                    return Response<Guid>.Fail("Invalid user ID");

                var trade = _tradeService.CreateTradeRequest(
                    requesterId,
                    request.RequestedTalentId,
                    request.OfferedTalentId,
                    request.Message);
                return Response<Guid>.Ok(trade.TradeId, "Trade request created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail($"Failed to create trade request: {ex.Message}");
            }
        }
        
        public async Task<BaseResponse> HandleAsync(UpdateTradeStatusRequest request)
        {
            if (!request.IsValid)
                return BaseResponse.Fail(string.Join(", ", request.Errors.Values));
            
            try
            {
                switch (request.Status)
                {
                    case "Accepted":
                        _tradeService.AcceptTradeRequest(request.TradeId);
                        break;
                    case "Rejected":
                        _tradeService.RejectTradeRequest(request.TradeId);
                        break;
                    case "Completed":
                        _tradeService.CompleteTrade(request.TradeId);
                        break;
                }
                return BaseResponse.Ok($"Trade {request.Status.ToLower()} successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse.Fail($"Failed to update trade status: {ex.Message}");
            }
        }
    }
}
