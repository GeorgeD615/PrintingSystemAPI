using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db.Models;

namespace PrintingSystem.Db;

public partial class PrintingSystemContext : DbContext
{
    public PrintingSystemContext()
    {
    }

    public PrintingSystemContext(DbContextOptions<PrintingSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConnectionType> ConnectionTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Installation> Installations { get; set; }

    public virtual DbSet<Macaddress> Macaddresses { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<PrintingDevice> PrintingDevices { get; set; }

    public virtual DbSet<SeccionStatus> SeccionStatuses { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PrintingSystem;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConnectionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Connecti__3213E83F20AD9D2E");

            entity.ToTable("ConnectionType");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F0A19019C");

            entity.ToTable("Employee");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");

            entity.HasOne(d => d.Office).WithMany(p => p.Employees)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__office__36B12243");
        });

        modelBuilder.Entity<Installation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Installa__3213E83FB92F9757");

            entity.ToTable("Installation");

            entity.HasIndex(e => e.OfficeId, "UQ_Installation_Default")
                .IsUnique()
                .HasFilter("([is_default]=(1))");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.InstallationOrderNumber).HasColumnName("installation_order_number");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.PrintingDeviceId).HasColumnName("printing_device_id");

            entity.HasOne(d => d.Office).WithMany(p => p.Installations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Installat__offic__32E0915F");

            entity.HasOne(d => d.PrintingDevice).WithMany(p => p.Installations)
                .HasForeignKey(d => d.PrintingDeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Installat__print__31EC6D26");
        });

        modelBuilder.Entity<Macaddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MACaddre__3213E83F22557A2B");

            entity.ToTable("MACaddresses");

            entity.Property(e => e.Id)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.PrintingDeviceId).HasColumnName("printing_device_id");

            entity.HasOne(d => d.PrintingDevice).WithMany(p => p.Macaddresses)
                .HasForeignKey(d => d.PrintingDeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MACaddres__print__2B3F6F97");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Office__3213E83F6E5C4728");

            entity.ToTable("Office");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PrintingDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Printing__3213E83F183B2976");

            entity.ToTable("PrintingDevice");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ConnectionTypeId).HasColumnName("connection_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.ConnectionType).WithMany(p => p.PrintingDevices)
                .HasForeignKey(d => d.ConnectionTypeId)
                .HasConstraintName("FK__PrintingD__conne__286302EC");
        });

        modelBuilder.Entity<SeccionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SeccionS__3213E83F35E9CD29");

            entity.ToTable("SeccionStatus");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Session__3213E83FE88FEE02");

            entity.ToTable("Session");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.InstallationId).HasColumnName("installation_id");
            entity.Property(e => e.NumberOfPages).HasColumnName("number_of_pages");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskName)
                .HasMaxLength(255)
                .HasColumnName("task_name");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Session__employe__3D5E1FD2");

            entity.HasOne(d => d.Installation).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.InstallationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Session__install__3E52440B");

            entity.HasOne(d => d.Status).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Session__status___3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
