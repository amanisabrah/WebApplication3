using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DB;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<USE_TYP_TypeOfRequset> USE_TYP_TypeOfRequset { get; set; }

    public virtual DbSet<USE_User> USE_User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<USE_TYP_TypeOfRequset>(entity =>
        {
            entity.HasKey(e => e.USE_TYP_ID);

            entity.Property(e => e.USE_TYP_MEssage).HasMaxLength(100);

            entity.HasOne(d => d.USE_TYP_User).WithMany(p => p.USE_TYP_TypeOfRequset)
                .HasForeignKey(d => d.USE_TYP_UserID)
                .HasConstraintName("FK_USE_TYP_TypeOfRequset_USE_User");
        });

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
