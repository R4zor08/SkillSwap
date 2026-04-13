namespace SkillSwap.Services.Interfaces;

public interface ISmsService
{
    SmsQueueItem Enqueue(string toPhone, string message, Guid requestedBy);
    IReadOnlyList<SmsQueueItem> GetPending(int take);
    SmsQueueItem? UpdateStatus(Guid id, string status, string? failureReason);
}

public class SmsQueueItem
{
    public Guid Id { get; set; }
    public string ToPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = SmsStatuses.Pending;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? SentAtUtc { get; set; }
    public string? FailureReason { get; set; }
    public Guid RequestedBy { get; set; }
}

public static class SmsStatuses
{
    public const string Pending = "pending";
    public const string Processing = "processing";
    public const string Sent = "sent";
    public const string Failed = "failed";
}
