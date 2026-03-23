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
    public class TradeService : ITradeService
    {
        private readonly ApplicationDbContext _context;

        public TradeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public TradeRequest CreateTradeRequest(Guid requesterId, Guid talentId)
        {
            var entity = new TradeRequestEntity
            {
                TradeId = Guid.NewGuid(),
                RequesterId = requesterId,
                RequestedTalentId = talentId,
                Status = (int)TradeStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };
            _context.TradeRequests.Add(entity);
            _context.SaveChanges();
            return new TradeRequest(entity.TradeId, entity.RequesterId, entity.RequestedTalentId, (TradeStatus)entity.Status, entity.RequestedAt, entity.Rating);
        }

        public TradeRequest CreateTradeRequest(Guid requesterId, Guid requestedTalentId, Guid offeredTalentId, string message)
        {
            var requestedTalent = _context.Talents.Find(requestedTalentId);
            if (requestedTalent == null) throw new ArgumentException("Requested talent not found");

            var entity = new TradeRequestEntity
            {
                TradeId = Guid.NewGuid(),
                RequesterId = requesterId,
                TargetStudentId = requestedTalent.StudentId,
                RequestedTalentId = requestedTalentId,
                OfferedTalentId = offeredTalentId,
                Status = (int)TradeStatus.Pending,
                RequestedAt = DateTime.UtcNow,
                Message = message
            };
            _context.TradeRequests.Add(entity);
            _context.SaveChanges();
            
            return CreateTradeRequestWithNavigation(entity);
        }

        public void ApproveTrade(Guid tradeId)
        {
            UpdateTradeStatus(tradeId, TradeStatus.Accepted);
        }

        public void RejectTrade(Guid tradeId)
        {
            UpdateTradeStatus(tradeId, TradeStatus.Rejected);
        }

        public void AcceptTradeRequest(Guid tradeId)
        {
            UpdateTradeStatus(tradeId, TradeStatus.Accepted);
        }

        public void CompleteTrade(Guid tradeId, int? rating = null)
        {
            var entity = _context.TradeRequests.Find(tradeId);
            if (entity == null) return;
            entity.Rating = rating;
            entity.Status = (int)TradeStatus.Completed;
            _context.SaveChanges();
        }

        public void RejectTradeRequest(Guid tradeId)
        {
            UpdateTradeStatus(tradeId, TradeStatus.Rejected);
        }

        public void CompleteTradeRequest(Guid tradeId)
        {
            UpdateTradeStatus(tradeId, TradeStatus.Completed);
        }

        public TradeRequest? GetTradeRequestById(Guid id)
        {
            return GetTradeById(id);
        }

        public IEnumerable<TradeRequest> GetTrades()
        {
            return _context.TradeRequests
                .AsNoTracking()
                .Include(t => t.Requester)
                .Include(t => t.TargetStudent)
                .Include(t => t.RequestedTalent)
                .Include(t => t.OfferedTalent)
                .ToList()
                .Select(e => CreateTradeRequestWithNavigation(e));
        }

        public TradeRequest? GetTradeById(Guid id)
        {
            var e = _context.TradeRequests
                .Include(t => t.Requester)
                .Include(t => t.TargetStudent)
                .Include(t => t.RequestedTalent)
                .Include(t => t.OfferedTalent)
                .FirstOrDefault(t => t.TradeId == id);
            
            return e == null ? null : CreateTradeRequestWithNavigation(e);
        }

        public IEnumerable<TradeRequest> GetIncomingTradeRequests(Guid studentId)
        {
            return _context.TradeRequests
                .AsNoTracking()
                .Include(t => t.Requester)
                .Include(t => t.TargetStudent)
                .Include(t => t.RequestedTalent)
                .Include(t => t.OfferedTalent)
                .Where(t => t.TargetStudentId == studentId)
                .ToList()
                .Select(e => CreateTradeRequestWithNavigation(e));
        }

        public IEnumerable<TradeRequest> GetOutgoingTradeRequests(Guid studentId)
        {
            return _context.TradeRequests
                .AsNoTracking()
                .Include(t => t.Requester)
                .Include(t => t.TargetStudent)
                .Include(t => t.RequestedTalent)
                .Include(t => t.OfferedTalent)
                .Where(t => t.RequesterId == studentId)
                .ToList()
                .Select(e => CreateTradeRequestWithNavigation(e));
        }

        private void UpdateTradeStatus(Guid tradeId, TradeStatus status)
        {
            var entity = _context.TradeRequests.Find(tradeId);
            if (entity == null) return;
            entity.Status = (int)status;
            _context.SaveChanges();
        }

        private TradeRequest CreateTradeRequestWithNavigation(TradeRequestEntity entity)
        {
            var tradeRequest = new TradeRequest(
                entity.TradeId, 
                entity.RequesterId, 
                entity.TargetStudentId, 
                entity.RequestedTalentId, 
                entity.OfferedTalentId, 
                (TradeStatus)entity.Status, 
                entity.RequestedAt, 
                entity.Message ?? string.Empty, 
                entity.Rating);

            // Set navigation properties
            if (entity.Requester != null)
                tradeRequest.RequestingStudent = new Student(entity.Requester.StudentId, entity.Requester.Name, entity.Requester.Email, entity.Requester.Password ?? string.Empty);
            
            if (entity.TargetStudent != null)
                tradeRequest.TargetStudent = new Student(entity.TargetStudent.StudentId, entity.TargetStudent.Name, entity.TargetStudent.Email, entity.TargetStudent.Password ?? string.Empty);
            
            if (entity.RequestedTalent != null)
                tradeRequest.RequestedTalent = new Talent(entity.RequestedTalent.TalentId, entity.RequestedTalent.TalentName, entity.RequestedTalent.Description, entity.RequestedTalent.StudentId, entity.RequestedTalent.ProficiencyLevel);
            
            if (entity.OfferedTalent != null)
                tradeRequest.OfferedTalent = new Talent(entity.OfferedTalent.TalentId, entity.OfferedTalent.TalentName, entity.OfferedTalent.Description, entity.OfferedTalent.StudentId, entity.OfferedTalent.ProficiencyLevel);

            return tradeRequest;
        }
    }
}
