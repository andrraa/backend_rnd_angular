using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RnD.Angular.Backend.Models;

public partial class LearnDbContext : DbContext
{
    public LearnDbContext()
    {
    }

    public LearnDbContext(DbContextOptions<LearnDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomerModel> TblCustomers { get; set; }

    public virtual DbSet<RefreshTokenModel> TblRefreshtokens { get; set; }

    public virtual DbSet<UserModel> TblUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerModel>(entity =>
        {
            entity.ToTable("tbl_customer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreditLimit).HasDefaultValueSql("((0))");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshTokenModel>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("tbl_refreshtoken");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TokenId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.HasKey(e => e.Userid);

            entity.ToTable("tbl_user");

            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
