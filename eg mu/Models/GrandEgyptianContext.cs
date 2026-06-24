using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eg_mu.Models;

public partial class GrandEgyptianContext : DbContext
{
    public GrandEgyptianContext()
    {
    }

    public GrandEgyptianContext(DbContextOptions<GrandEgyptianContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Artifact> Artifacts { get; set; }

    public virtual DbSet<ArtifactsExhibition> ArtifactsExhibitions { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ConservationLab> ConservationLabs { get; set; }

    public virtual DbSet<DailyTask> DailyTasks { get; set; }

    public virtual DbSet<Exhibition> Exhibitions { get; set; }

    public virtual DbSet<Garden> Gardens { get; set; }

    public virtual DbSet<MuseumSection> MuseumSections { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<TicketPrice> TicketPrices { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }
    public object Staffs { get; internal set; }
    public object Attendance { get; internal set; }
    public IEnumerable<object> Tickets { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E89CBE3E34");

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534651BC863").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(200);
        });

        modelBuilder.Entity<Artifact>(entity =>
        {
            entity.HasKey(e => e.ArtifactId).HasName("PK__Artifact__E788EA9606BA700C");

            entity.Property(e => e.ArtifactId).HasColumnName("ArtifactID");
            entity.Property(e => e.Era).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");
            entity.Property(e => e.LabId).HasColumnName("LabID");
            entity.Property(e => e.Material).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Lab).WithMany(p => p.Artifacts)
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK_Artifacts_ConservationLabs");

            entity.HasOne(d => d.Section).WithMany(p => p.Artifacts)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Artifacts_Sections");
        });

        modelBuilder.Entity<ArtifactsExhibition>(entity =>
        {
            entity.HasKey(e => new { e.ArtifactId, e.ExhibitionId }).HasName("PK__Artifact__14A43651826F0E4A");

            entity.Property(e => e.ArtifactId).HasColumnName("ArtifactID");
            entity.Property(e => e.ExhibitionId).HasColumnName("ExhibitionID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Artifact).WithMany(p => p.ArtifactsExhibitions)
                .HasForeignKey(d => d.ArtifactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Artifacts__Artif__6477ECF3");

            entity.HasOne(d => d.Exhibition).WithMany(p => p.ArtifactsExhibitions)
                .HasForeignKey(d => d.ExhibitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Artifacts__Exhib__656C112C");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263CEF699885");

            entity.ToTable("Attendance");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CheckIn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckOut).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");

            entity.HasOne(d => d.Staff).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Staff");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD8464F14E");

            entity.ToTable(tb => tb.HasTrigger("trg_CalculateTotal"));

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.AgeCategory).HasMaxLength(50);
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsStudent).HasDefaultValue(false);
            entity.Property(e => e.Nationality).HasMaxLength(50);
            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VisitorId).HasColumnName("VisitorID");

            entity.HasOne(d => d.Price).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Prices");

            entity.HasOne(d => d.Visitor).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Visitors");
        });

        modelBuilder.Entity<ConservationLab>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("PK__Conserva__EDBD773A2BE53AA7");

            entity.Property(e => e.LabId).HasColumnName("LabID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Section).WithMany(p => p.ConservationLabs)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConservationLabs_Sections");
        });

        modelBuilder.Entity<DailyTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__DailyTas__7C6949D119972A8D");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.TaskDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Staff).WithMany(p => p.DailyTasks)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Staff");
        });

        modelBuilder.Entity<Exhibition>(entity =>
        {
            entity.HasKey(e => e.ExhibitionId).HasName("PK__Exhibiti__32CDCC7EEC518E2C");

            entity.Property(e => e.ExhibitionId).HasColumnName("ExhibitionID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Section).WithMany(p => p.Exhibitions)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exhibitions_Sections");
        });

        modelBuilder.Entity<Garden>(entity =>
        {
            entity.HasKey(e => e.GardenId).HasName("PK__Gardens__0191D063819C7315");

            entity.Property(e => e.GardenId).HasColumnName("GardenID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Section).WithMany(p => p.Gardens)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Gardens_Sections");
        });

        modelBuilder.Entity<MuseumSection>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__MuseumSe__80EF08927B905AA5");

            entity.Property(e => e.SectionId).HasColumnName("SectionID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7F933DF4A");

            entity.HasIndex(e => e.Email, "UQ__Staff__A9D10534DD440527").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.HireDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Section).WithMany(p => p.Staff)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Staff_Sections");
        });

        modelBuilder.Entity<TicketPrice>(entity =>
        {
            entity.HasKey(e => e.PriceId).HasName("PK__TicketPr__4957584FDE104E7D");

            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.VisitorId).HasName("PK__Visitors__B121AFA825C4740C");

            entity.HasIndex(e => e.Email, "UQ__Visitors__A9D10534558396D0").IsUnique();

            entity.Property(e => e.VisitorId).HasColumnName("VisitorID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
