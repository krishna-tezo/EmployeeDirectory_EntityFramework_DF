using EmployeeDirectory.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirectory.Data.Models;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }


    public override int SaveChanges()
    {
        SetAuditProperties();
        return base.SaveChanges();
    }

    //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    //{
    //    SetAuditProperties();
    //    return base.SaveChanges(acceptAllChangesOnSuccess, cancellationToken);
    //}

    private void SetAuditProperties()
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable entity)
            {
                var now = DateOnly.FromDateTime(DateTime.UtcNow);
                var user = "System";

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedBy = user;
                        entity.ModifiedBy = user;
                        entity.CreatedDate = now;
                        entity.ModifiedDate = now;
                        break;

                    case EntityState.Modified:
                        entity.ModifiedBy = user;
                        entity.ModifiedDate = now;
                        break;
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0763784093");

            entity.ToTable("Department");

            entity.HasIndex(e => e.Id, "UQ__Departme__3214EC06C08C40B3").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07F645C477");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.Id, "UQ__Employee__3214EC064834EE06").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasDefaultValue("System");
            entity.Property(e => e.CreatedDate).HasDefaultValue(new DateOnly(2016, 4, 25));
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasDefaultValue("System");
            entity.Property(e => e.ModifiedDate).HasDefaultValue(new DateOnly(2016, 4, 25));
            entity.Property(e => e.ProjectId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.RoleId)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.HasOne(d => d.Project).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_ProjectId");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Employee__RoleId__45F365D3");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC077EA56EEE");

            entity.ToTable("Location");

            entity.HasIndex(e => e.Id, "UQ__Location__3214EC060D6EBD8B").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC078043A0CC");

            entity.ToTable("Manager");

            entity.HasIndex(e => e.Id, "UQ__Manager__3214EC06976F0215").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithMany(p => p.Managers)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK_EmpId");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC0713F2394D");

            entity.ToTable("Project");

            entity.Property(e => e.Id)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.ManagerId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Manager).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Project__Manager__47DBAE45");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC076B7EBB60");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Id, "UQ__Role__3214EC06F7218EBE").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LocationId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Roles)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Role__Department__48CFD27E");

            entity.HasOne(d => d.Location).WithMany(p => p.Roles)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Role__LocationId__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
