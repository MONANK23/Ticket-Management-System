using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TMS.Models;

[Table("ticket_comments")]
public partial class TicketComment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("comment", TypeName = "text")]
    public string Comment { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("TicketId")]
    [InverseProperty("TicketComments")]
    public virtual Ticket Ticket { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("TicketComments")]
    public virtual User User { get; set; } = null!;
}
