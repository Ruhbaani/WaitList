using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaitListWeb.Data;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Controllers;

[ApiController]
[Route("api/queues/{queueId:int}/entries")]
[Authorize]
public class QueueEntryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QueueEntryController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int? GetCurrentAccountId()
    {
        var claim = User.FindFirst(ApplicationClaimsPrincipalFactory.AccountIdClaimType)?.Value
                    ?? User.FindFirst("account_id")?.Value;

        return int.TryParse(claim, out var accountId) ? accountId : null;
    }

    private async Task<Queue?> GetAccessibleQueueAsync(int queueId)
    {
        var currentAccountId = GetCurrentAccountId();

        IQueryable<Queue> query = _context.Queues.Where(q => q.QueueId == queueId);

        if (!User.IsInRole(AppRoles.SystemAdmin))
        {
            if (currentAccountId is null)
                return null;

            query = query.Where(q => q.AccountId == currentAccountId.Value);
        }

        return await query.FirstOrDefaultAsync();
    }

    [HttpGet]
    public async Task<IActionResult> GetEntries(int queueId)
    {
        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entries = await _context.QueueEntries
            .Where(e => e.QueueId == queueId)
            .OrderBy(e => e.Position)
            .Select(e => new
            {
                e.QueueEntryId,
                e.QueueId,
                e.CustomerId,
                e.ServiceId,
                e.CreatedAt,
                e.IsNotified,
                e.IsServed,
                e.Position
            })
            .ToListAsync();

        return Ok(entries);
    }

    [HttpGet("{entryId:int}")]
    public async Task<IActionResult> GetEntryById(int queueId, int entryId)
    {
        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entry = await _context.QueueEntries
            .FirstOrDefaultAsync(e => e.QueueEntryId == entryId && e.QueueId == queueId);

        if (entry is null)
            return NotFound();

        return Ok(entry);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager},{AppRoles.Server}")]
    public async Task<IActionResult> AddEntry(int queueId, [FromBody] CreateQueueEntryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == dto.CustomerId && c.AccountId == queue.AccountId);

        if (customer is null)
            return BadRequest(new { message = "Customer not found for this account." });

        if (dto.ServiceId.HasValue)
        {
            var serviceExists = await _context.Services
                .AnyAsync(s => s.ServiceId == dto.ServiceId.Value && s.AccountId == queue.AccountId);

            if (!serviceExists)
                return BadRequest(new { message = "Service not found for this account." });
        }

        var maxPosition = await _context.QueueEntries
            .Where(e => e.QueueId == queueId)
            .Select(e => (int?)e.Position)
            .MaxAsync() ?? 0;

        var entry = new QueueEntry
        {
            AccountId = queue.AccountId,
            QueueId = queueId,
            CustomerId = dto.CustomerId,
            ServiceId = dto.ServiceId,
            CreatedAt = DateTime.UtcNow,
            IsNotified = false,
            IsServed = false,
            Position = maxPosition + 1
        };

        _context.QueueEntries.Add(entry);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEntryById), new { queueId, entryId = entry.QueueEntryId }, entry);
    }

    [HttpDelete("{entryId:int}")]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager},{AppRoles.Server}")]
    public async Task<IActionResult> RemoveEntry(int queueId, int entryId)
    {
        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entry = await _context.QueueEntries
            .FirstOrDefaultAsync(e => e.QueueEntryId == entryId && e.QueueId == queueId);

        if (entry is null)
            return NotFound();

        _context.QueueEntries.Remove(entry);
        await _context.SaveChangesAsync();

        var remainingEntries = await _context.QueueEntries
            .Where(e => e.QueueId == queueId)
            .OrderBy(e => e.Position)
            .ToListAsync();

        for (int i = 0; i < remainingEntries.Count; i++)
            remainingEntries[i].Position = i + 1;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("notify-next")]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager},{AppRoles.Server}")]
    public async Task<IActionResult> NotifyNext(int queueId)
    {
        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entry = await _context.QueueEntries
            .Where(e => e.QueueId == queueId && !e.IsServed && !e.IsNotified)
            .OrderBy(e => e.Position)
            .FirstOrDefaultAsync();

        if (entry is null)
            return NotFound(new { message = "No waiting customer found." });

        entry.IsNotified = true;
        await _context.SaveChangesAsync();

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == entry.CustomerId);

        return Ok(new
        {
            message = "Customer notified.",
            entry.QueueEntryId,
            entry.Position,
            customer = customer is null ? null : new
            {
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.Phone
            }
        });
    }

    [HttpPost("{entryId:int}/serve")]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager},{AppRoles.Server}")]
    public async Task<IActionResult> MarkServed(int queueId, int entryId)
    {
        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entry = await _context.QueueEntries
            .FirstOrDefaultAsync(e => e.QueueEntryId == entryId && e.QueueId == queueId);

        if (entry is null)
            return NotFound();

        entry.IsServed = true;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Entry marked as served.",
            entry.QueueEntryId
        });
    }

    [HttpPost("reorder")]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager}")]
    public async Task<IActionResult> Reorder(int queueId, [FromBody] ReorderQueueEntriesDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var queue = await GetAccessibleQueueAsync(queueId);
        if (queue is null)
            return NotFound();

        var entries = await _context.QueueEntries
            .Where(e => e.QueueId == queueId)
            .ToListAsync();

        if (dto.EntryIdsInOrder.Count != entries.Count)
            return BadRequest(new { message = "Submitted order does not match queue entry count." });

        var entryIdsSet = entries.Select(e => e.QueueEntryId).OrderBy(x => x).ToList();
        var dtoIdsSet = dto.EntryIdsInOrder.OrderBy(x => x).ToList();

        if (!entryIdsSet.SequenceEqual(dtoIdsSet))
            return BadRequest(new { message = "Submitted order contains invalid queue entry IDs." });

        for (int i = 0; i < dto.EntryIdsInOrder.Count; i++)
        {
            var entry = entries.First(e => e.QueueEntryId == dto.EntryIdsInOrder[i]);
            entry.Position = i + 1;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Queue reordered successfully." });
    }
}