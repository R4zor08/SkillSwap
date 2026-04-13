using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Web.Controllers;

[ApiController]
[Route("api/sms")]
[Authorize]
public class SmsApiController : ControllerBase
{
    private readonly ISmsService _smsService;

    public SmsApiController(ISmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost("notifications")]
    public IActionResult QueueNotification([FromBody] SmsNotificationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.ToPhone) || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { error = "toPhone and message are required." });

        if (request.Message.Trim().Length > 480)
            return BadRequest(new { error = "message must be 480 characters or less." });

        var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!Guid.TryParse(userIdRaw, out var requestedBy))
            return Unauthorized();

        var item = _smsService.Enqueue(request.ToPhone.Trim(), request.Message.Trim(), requestedBy);
        return Ok(new SmsQueueResponse(item));
    }

    [HttpGet("gateway/pending")]
    public IActionResult GetPending([FromQuery] int take = 1)
    {
        var pending = _smsService.GetPending(take)
            .Select(x => new SmsGatewayPendingItem
            {
                Id = x.Id,
                ToPhone = x.ToPhone,
                Message = x.Message
            })
            .ToList();

        return Ok(pending);
    }

    [HttpPut("gateway/status/{id:guid}")]
    public IActionResult UpdateStatus(Guid id, [FromBody] SmsGatewayStatusUpdateRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Status))
            return BadRequest(new { error = "status is required." });

        try
        {
            var updated = _smsService.UpdateStatus(id, request.Status, request.FailureReason);
            if (updated == null)
                return NotFound();

            return Ok(new SmsQueueResponse(updated));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class SmsNotificationRequest
{
    public string ToPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SmsGatewayPendingItem
{
    public Guid Id { get; set; }
    public string ToPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SmsGatewayStatusUpdateRequest
{
    public string Status { get; set; } = string.Empty;
    public string? FailureReason { get; set; }
}

public class SmsQueueResponse
{
    public SmsQueueResponse(SmsQueueItem item)
    {
        Id = item.Id;
        ToPhone = item.ToPhone;
        Message = item.Message;
        Status = item.Status;
        CreatedAtUtc = item.CreatedAtUtc;
        SentAtUtc = item.SentAtUtc;
        FailureReason = item.FailureReason;
        RequestedBy = item.RequestedBy;
    }

    public Guid Id { get; }
    public string ToPhone { get; }
    public string Message { get; }
    public string Status { get; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? SentAtUtc { get; }
    public string? FailureReason { get; }
    public Guid RequestedBy { get; }
}
