using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DB;

public partial class VtechTicketingDbContext : DbContext
{
    public VtechTicketingDbContext(DbContextOptions<VtechTicketingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<USE_User> USE_User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<USE_User>(entity =>
        {
            entity.HasKey(e => e.USE_User_ID);

            entity.Property(e => e.USE_User_Email).HasMaxLength(100);
            entity.Property(e => e.USE_User_Name).HasMaxLength(100);
            entity.Property(e => e.USE_User_Password).HasMaxLength(100);
            entity.Property(e => e.USE_User_Phone).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
