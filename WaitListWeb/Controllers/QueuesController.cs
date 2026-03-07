using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaitListWeb.Data;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QueuesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QueuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int? GetCurrentAccountId()
    {
        var claim = User.FindFirst(ApplicationClaimsPrincipalFactory.AccountIdClaimType)?.Value
                    ?? User.FindFirst("account_id")?.Value;

        return int.TryParse(claim, out var accountId) ? accountId : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetQueues()
    {
        var currentAccountId = GetCurrentAccountId();

        IQueryable<Queue> query = _context.Queues;

        if (!User.IsInRole(AppRoles.SystemAdmin))
        {
            if (currentAccountId is null)
                return Unauthorized();

            query = query.Where(q => q.AccountId == currentAccountId.Value);
        }

        var queues = await query.ToListAsync();
        return Ok(queues);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQueueById(int id)
    {
        var currentAccountId = GetCurrentAccountId();

        IQueryable<Queue> query = _context.Queues.Where(q => q.QueueId == id);

        if (!User.IsInRole(AppRoles.SystemAdmin))
        {
            if (currentAccountId is null)
                return Unauthorized();

            query = query.Where(q => q.AccountId == currentAccountId.Value);
        }

        var queue = await query.FirstOrDefaultAsync();

        if (queue is null)
            return NotFound();

        return Ok(queue);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager}")]
    public async Task<IActionResult> CreateQueue([FromBody] CreateQueueDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentAccountId = GetCurrentAccountId();
        if (!User.IsInRole(AppRoles.SystemAdmin) && currentAccountId is null)
            return Unauthorized();

        var queue = new Queue
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            AccountId = User.IsInRole(AppRoles.SystemAdmin) ? dto.AccountId : currentAccountId!.Value
        };

        _context.Queues.Add(queue);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQueueById), new { id = queue.QueueId }, queue);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner},{AppRoles.Manager}")]
    public async Task<IActionResult> DeleteQueue(int id)
    {
        var currentAccountId = GetCurrentAccountId();

        IQueryable<Queue> query = _context.Queues.Where(q => q.QueueId == id);

        if (!User.IsInRole(AppRoles.SystemAdmin))
        {
            if (currentAccountId is null)
                return Unauthorized();

            query = query.Where(q => q.AccountId == currentAccountId.Value);
        }

        var queue = await query.FirstOrDefaultAsync();
        if (queue is null)
            return NotFound();

        var entries = await _context.QueueEntries
            .Where(e => e.QueueId == id)
            .ToListAsync();

        if (entries.Count > 0)
            _context.QueueEntries.RemoveRange(entries);

        _context.Queues.Remove(queue);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}