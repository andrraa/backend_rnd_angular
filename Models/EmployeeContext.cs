using API.EmployeeManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.EmployeeManagement.Models;

public partial class EmployeeContext : DbContext
{
    public EmployeeContext()
    {
    }

    public EmployeeContext(DbContextOptions<EmployeeContext> options)
    : base(options)
    {
    }

    public virtual DbSet<EmployeeVM> Employees { get; set; }

    public virtual DbSet<UserAccountVM> Useraccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeVM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83F589F9FF8");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Divisi)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("divisi");
            entity.Property(e => e.Kelompok)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("kelompok");
            entity.Property(e => e.Nama)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nama");
            entity.Property(e => e.Npp).HasColumnName("npp");
        });

        modelBuilder.Entity<UserAccountVM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__useracco__3213E83F5724CAEF");

            entity.ToTable("useraccount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkEmployee).HasColumnName("fk_employee");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
