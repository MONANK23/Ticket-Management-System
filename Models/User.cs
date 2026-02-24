using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TMS.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("AssignedToNavigation")]
    public virtual ICollection<Ticket> TicketAssignedToNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("User")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Ticket> TicketCreatedByNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("ChangedByNavigation")]
    public virtual ICollection<TicketStatusLog> TicketStatusLogs { get; set; } = new List<TicketStatusLog>();
}
