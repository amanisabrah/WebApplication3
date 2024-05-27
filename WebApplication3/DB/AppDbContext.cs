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

    public virtual DbSet<AAA_REQ_Requset> AAA_REQ_Requset { get; set; }

    public virtual DbSet<AAA_USR_User> AAA_USR_User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AAA_REQ_Requset>(entity =>
        {
            entity.HasKey(e => e.AAA_REQ_ID).HasName("PK_USE_TYP_TypeOfRequset");

            entity.Property(e => e.AAA_REQ_EntryDate).HasPrecision(0);
            entity.Property(e => e.AAA_REQ_Message).HasMaxLength(100);

            entity.HasOne(d => d.AAA_REQ_USRID_EntryNavigation).WithMany(p => p.AAA_REQ_Requset)
                .HasForeignKey(d => d.AAA_REQ_USRID_Entry)
                .HasConstraintName("FK_USE_TYP_TypeOfRequset_USE_User");
        });

        modelBuilder.Entity<AAA_USR_User>(entity =>
        {
            entity.HasKey(e => e.AAA_USR_ID).HasName("PK_USE_User");

            entity.Property(e => e.AAA_USR_Email).HasMaxLength(100);
            entity.Property(e => e.AAA_USR_Name).HasMaxLength(100);
            entity.Property(e => e.AAA_USR_Password).HasMaxLength(100);
            entity.Property(e => e.AAA_USR_Phone).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
