namespace TMS.Models;

public class UserCreateDTO
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public int RoleId { get; set; }
}

public class TicketCreateDTO
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Priority { get; set; } = "MEDIUM";
    public int CreatedBy { get; set; }
}

public class TicketAssignDTO
{
    public int AssignedTo { get; set; }
}

public class TicketStatusUpdateDTO
{
    public string Status { get; set; } = "";
    public int ChangedBy { get; set; }
}

public class CommentCreateDTO
{
    public string Comment { get; set; } = "";
    public int UserId { get; set; }
}

public class LoginDTO
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
