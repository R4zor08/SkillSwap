using System.Collections.Concurrent;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Services;

public class SmsService : ISmsService
{
    private readonly ConcurrentDictionary<Guid, SmsQueueItem> _items = new();
    private readonly object _sync = new();

    public SmsQueueItem Enqueue(string toPhone, string message, Guid requestedBy)
    {
        var item = new SmsQueueItem
        {
            Id = Guid.NewGuid(),
            ToPhone = toPhone,
            Message = message,
            Status = SmsStatuses.Pending,
            CreatedAtUtc = DateTime.UtcNow,
            RequestedBy = requestedBy
        };

        _items[item.Id] = item;
        return item;
    }

    public IReadOnlyList<SmsQueueItem> GetPending(int take)
    {
        if (take < 1) take = 1;
        if (take > 50) take = 50;

        lock (_sync)
        {
            var pending = _items.Values
                .Where(x => x.Status == SmsStatuses.Pending)
                .OrderBy(x => x.CreatedAtUtc)
                .Take(take)
                .ToList();

            foreach (var item in pending)
            {
                item.Status = SmsStatuses.Processing;
                _items[item.Id] = item;
            }

            return pending;
        }
    }

    public SmsQueueItem? UpdateStatus(Guid id, string status, string? failureReason)
    {
        if (!_items.TryGetValue(id, out var item))
            return null;

        var normalized = status.Trim().ToLowerInvariant();
        if (normalized != SmsStatuses.Sent &&
            normalized != SmsStatuses.Failed &&
            normalized != SmsStatuses.Pending &&
            normalized != SmsStatuses.Processing)
            throw new ArgumentException("Invalid status value.", nameof(status));

        item.Status = normalized;
        item.FailureReason = string.IsNullOrWhiteSpace(failureReason) ? null : failureReason.Trim();
        if (normalized == SmsStatuses.Sent)
            item.SentAtUtc = DateTime.UtcNow;

        _items[id] = item;
        return item;
    }
}
