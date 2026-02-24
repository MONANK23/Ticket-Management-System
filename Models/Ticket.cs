using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TMS.Models;

[Table("tickets")]
public partial class Ticket
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column("priority")]
    [StringLength(50)]
    [Unicode(false)]
    public string Priority { get; set; } = null!;

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("assigned_to")]
    public int? AssignedTo { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AssignedTo")]
    [InverseProperty("TicketAssignedToNavigations")]
    public virtual User? AssignedToNavigation { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TicketCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketStatusLog> TicketStatusLogs { get; set; } = new List<TicketStatusLog>();
}
