using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.Models;

namespace TMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly TMSContext _context;

    public TicketsController(TMSContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<Ticket> GetTickets()
    {
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

        if (role == "MANAGER")
        {
            return _context.Tickets.ToList();
        }
        else if (role == "SUPPORT")
        {
            return _context.Tickets.Where(t => t.AssignedTo == userId).ToList();
        }
        else
        {
            return _context.Tickets.Where(t => t.CreatedBy == userId).ToList();
        }
    }

    [HttpPost]
    [Authorize(Roles = "USER,MANAGER")]
    public ActionResult<Ticket> PostTicket(TicketCreateDTO ticketDto)
    {
        Ticket t = new Ticket();
        t.Title = ticketDto.Title;
        t.Description = ticketDto.Description;
        t.Priority = ticketDto.Priority;
        t.Status = "OPEN";
        t.CreatedBy = ticketDto.CreatedBy;
        t.CreatedAt = DateTime.Now;

        _context.Tickets.Add(t);
        _context.SaveChanges();

        return t;
    }

    [HttpPatch("{id}/assign")]
    [Authorize(Roles = "MANAGER,SUPPORT")]
    public ActionResult<string> AssignTicket(int id, TicketAssignDTO assignDto)
    {
        var t = _context.Tickets.Find(id);
        if (t == null) return NotFound("Ticket Not Found");

        var targetUser = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == assignDto.AssignedTo);
        if (targetUser == null || targetUser.Role.Name == "USER")
        {
            return BadRequest("Cannot assign ticket to a regular USER.");
        }

        t.AssignedTo = assignDto.AssignedTo;
        _context.SaveChanges();

        return Ok("Assigned Successfully");
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "MANAGER,SUPPORT")]
    public ActionResult<string> UpdateStatus(int id, TicketStatusUpdateDTO statusDto)
    {
        var t = _context.Tickets.Find(id);
        if (t == null) return NotFound("Ticket Not Found");

        
        bool isValid = false;
        if (t.Status == "OPEN" && statusDto.Status == "IN_PROGRESS") isValid = true;
        else if (t.Status == "IN_PROGRESS" && statusDto.Status == "RESOLVED") isValid = true;
        else if (t.Status == "RESOLVED" && statusDto.Status == "CLOSED") isValid = true;

        if (!isValid)
        {
            return BadRequest($"Invalid status transition from {t.Status} to {statusDto.Status}.");
        }

        string old = t.Status;
        t.Status = statusDto.Status;

        TicketStatusLog log = new TicketStatusLog();
        log.TicketId = id;
        log.OldStatus = old;
        log.NewStatus = statusDto.Status;
        log.ChangedBy = statusDto.ChangedBy;
        log.ChangedAt = DateTime.Now;

        _context.TicketStatusLogs.Add(log);
        _context.SaveChanges();

        return Ok("Status Updated");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "MANAGER")]
    public ActionResult<string> DeleteTicket(int id)
    {
        var t = _context.Tickets.Find(id);
        if (t == null) return NotFound("Not Found");

        _context.Tickets.Remove(t);
        _context.SaveChanges();

        return Ok("Deleted");
    }
}

