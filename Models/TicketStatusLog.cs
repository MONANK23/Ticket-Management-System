using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TMS.Models;

[Table("ticket_status_logs")]
public partial class TicketStatusLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("old_status")]
    [StringLength(50)]
    [Unicode(false)]
    public string OldStatus { get; set; } = null!;

    [Column("new_status")]
    [StringLength(50)]
    [Unicode(false)]
    public string NewStatus { get; set; } = null!;

    [Column("changed_by")]
    public int ChangedBy { get; set; }

    [Column("changed_at", TypeName = "datetime")]
    public DateTime? ChangedAt { get; set; }

    [ForeignKey("ChangedBy")]
    [InverseProperty("TicketStatusLogs")]
    public virtual User ChangedByNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("TicketStatusLogs")]
    public virtual Ticket Ticket { get; set; } = null!;
}
