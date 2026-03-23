using System;
using System.Collections.Generic;
using SkillSwap.Models;

namespace SkillSwap.Services.Interfaces
{
    public interface ITradeService
    {
        TradeRequest CreateTradeRequest(Guid requesterId, Guid talentId);
        TradeRequest CreateTradeRequest(Guid requesterId, Guid requestedTalentId, Guid offeredTalentId, string message);
        void ApproveTrade(Guid tradeId);
        void RejectTrade(Guid tradeId);
        void RejectTradeRequest(Guid tradeId);
        void AcceptTradeRequest(Guid tradeId);
        void CompleteTrade(Guid tradeId, int? rating = null);
        void CompleteTradeRequest(Guid tradeId);
        IEnumerable<TradeRequest> GetTrades();
        TradeRequest? GetTradeById(Guid id);
        TradeRequest? GetTradeRequestById(Guid id);
        IEnumerable<TradeRequest> GetIncomingTradeRequests(Guid studentId);
        IEnumerable<TradeRequest> GetOutgoingTradeRequests(Guid studentId);
    }
}
