using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TMS.Models;

public partial class TMSContext : DbContext
{
    public TMSContext()
    {
    }

    public TMSContext(DbContextOptions<TMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketComment> TicketComments { get; set; }

    public virtual DbSet<TicketStatusLog> TicketStatusLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F939D5E7B");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tickets__3213E83F2A8B5C8D");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TicketAssignedToNavigations).HasConstraintName("FK__tickets__assigne__3C69FB99");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TicketCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tickets__created__3B75D760");
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ticket_c__3213E83F71F878C1");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ticket_co__ticke__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.TicketComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ticket_co__user___403A8C7D");
        });

        modelBuilder.Entity<TicketStatusLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ticket_s__3213E83FB53A9CF8");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.TicketStatusLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ticket_st__chang__440B1D61");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketStatusLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ticket_st__ticke__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F2AA9A4C3");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__38996AB5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
