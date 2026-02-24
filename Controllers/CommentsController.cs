using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Models;

namespace TMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly TMSContext _context;

    public CommentsController(TMSContext context)
    {
        _context = context;
    }

    [HttpGet("/api/tickets/{ticketId}/comments")]
    public ActionResult<List<TicketComment>> GetComments(int ticketId)
    {
        var ticket = _context.Tickets.Find(ticketId);
        if (ticket == null) return NotFound("Ticket not found");

        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

        if (role != "MANAGER" && ticket.CreatedBy != userId && ticket.AssignedTo != userId)
        {
            return Forbid();
        }

        var results = _context.TicketComments.Where(c => c.TicketId == ticketId).ToList();
        return results;
    }

    [HttpPost("/api/tickets/{ticketId}/comments")]
    public ActionResult<TicketComment> PostComment(int ticketId, CommentCreateDTO commentDto)
    {
        var ticket = _context.Tickets.Find(ticketId);
        if (ticket == null) return NotFound("Ticket not found");

        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

        if (role != "MANAGER" && ticket.CreatedBy != userId && ticket.AssignedTo != userId)
        {
            return Forbid();
        }

        TicketComment c = new TicketComment();
        c.TicketId = ticketId;
        c.UserId = userId; 
        c.Comment = commentDto.Comment;
        c.CreatedAt = DateTime.Now;

        _context.TicketComments.Add(c);
        _context.SaveChanges();

        return c;
    }

    [HttpPatch("{id}")]
    public ActionResult<string> UpdateComment(int id, CommentCreateDTO commentDto)
    {
        var c = _context.TicketComments.Find(id);
        if (c == null) return NotFound("Comment not found");

        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

        if (role != "MANAGER" && c.UserId != userId)
        {
            return Forbid();
        }

        c.Comment = commentDto.Comment;
        _context.SaveChanges();

        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public ActionResult<string> DeleteComment(int id)
    {
        var c = _context.TicketComments.Find(id);
        if (c == null) return NotFound("Comment not found");

        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

        if (role != "MANAGER" && c.UserId != userId)
        {
            return Forbid();
        }

        _context.TicketComments.Remove(c);
        _context.SaveChanges();

        return Ok("Deleted");
    }
}
