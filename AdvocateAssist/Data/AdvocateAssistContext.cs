using System;
using System.Collections.Generic;
using AdvocateAssist.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvocateAssist.Data;

public partial class AdvocateAssistContext : DbContext
{
    public AdvocateAssistContext()
    {
    }

    public AdvocateAssistContext(DbContextOptions<AdvocateAssistContext> options)
        : base(options)
    {
    }

 

    public virtual DbSet<Case> Cases { get; set; }


    public virtual DbSet<LegalAssistant> LegalAssistants { get; set; }

    public virtual DbSet<LegalAssistantCase> LegalAssistantCases { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<LegalAssistant>().HasData(
            new LegalAssistant
            {
                LegalAssistantId = 1,
                LegalAssistantFname = "Jony",
                LegalAssistantLname = "Sarowar",
                JoinDate = new DateTime(2022, 5, 10),
                Email = "jony@gmail.com",
                MobileNo = "01710000001",
                IsActive = true,
                MonthlyStipend = 15000,
                NidNumber = "1234567890",
                Picture = "jony.jpg",
                BarLicenseNumber = "BAR1234",
                Division = "Dhaka",
                District = "Dhaka",
                City = "Mirpur"
            },
            new LegalAssistant
            {
                LegalAssistantId = 2,
                LegalAssistantFname = "Rafiq",
                LegalAssistantLname = "Hasan",
                JoinDate = new DateTime(2023, 3, 15),
                Email = "rafiq@gmail.com",
                MobileNo = "01710000002",
                IsActive = true,
                MonthlyStipend = 12000,
                NidNumber = "1234567890",
                Picture = "rafiq.png",
                BarLicenseNumber = "BAR5678",
                Division = "Chittagong",
                District = "Chittagong",
                City = "Agrabad"
            }
        );

        modelBuilder.Entity<Case>().HasData(
            new Case
            {
                CaseId = 1,
                CaseNumber = "CASE-2024-01"
            },
            new Case
            {
                CaseId = 2,
                CaseNumber = "CASE-2024-02"
            }
        );

        modelBuilder.Entity<LegalAssistantCase>().HasData(
            new LegalAssistantCase
            {
                LegalAssistantCaseId = 1,
                LegalAssistantId = 1,
                CaseId = 1,
                CaseTitle = "Land Dispute Resolution",
                Firnumber = "FIR-001",
                FilingDate = new DateTime(2024, 2, 20)
            },
            new LegalAssistantCase
            {
                LegalAssistantCaseId = 2,
                LegalAssistantId = 2,
                CaseId = 2,
                CaseTitle = "Family Property Claim",
                Firnumber = "FIR-002",
                FilingDate = new DateTime(2024, 4, 5)
            }

        );

      

        modelBuilder.Entity<Case>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK__Case__6CAE524CC2674450");

            entity.ToTable("Case");

            entity.Property(e => e.CaseNumber).HasMaxLength(100);
        });


        modelBuilder.Entity<LegalAssistant>(entity =>
        {
            entity.HasKey(e => e.LegalAssistantId).HasName("PK__LegalAss__C876325AA5C50EDD");

            entity.ToTable("LegalAssistant");

            entity.Property(e => e.BarLicenseNumber).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Division).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.LegalAssistantFname)
                .HasMaxLength(100)
                .HasColumnName("LegalAssistantFName");
            entity.Property(e => e.LegalAssistantLname)
                .HasMaxLength(100)
                .HasColumnName("LegalAssistantLName");
            entity.Property(e => e.MobileNo).HasMaxLength(20);
            entity.Property(e => e.MonthlyStipend).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<LegalAssistantCase>(entity =>
        {
            entity.HasKey(e => e.LegalAssistantCaseId).HasName("PK__LegalAss__8C4D842CFFEBE815");

            entity.ToTable("LegalAssistantCase");

            entity.Property(e => e.CaseTitle).HasMaxLength(200);
            entity.Property(e => e.FilingDate).HasColumnType("datetime");
            entity.Property(e => e.Firnumber)
                .HasMaxLength(50)
                .HasColumnName("FIRNumber");

            entity.HasOne(d => d.Case).WithMany(p => p.LegalAssistantCases)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LegalAssi__CaseI__286302EC");

            entity.HasOne(d => d.LegalAssistant).WithMany(p => p.LegalAssistantCases)
                .HasForeignKey(d => d.LegalAssistantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LegalAssi__Legal__29572725");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
