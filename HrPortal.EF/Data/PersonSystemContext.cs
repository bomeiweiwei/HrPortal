using System;
using System.Collections.Generic;
using HrPortal.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace HrPortal.EF.Data;

public partial class PersonSystemContext : DbContext
{
    public PersonSystemContext(DbContextOptions<PersonSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AppPermission> AppPermissions { get; set; }

    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppRolePermission> AppRolePermissions { get; set; }

    public virtual DbSet<AppUserRole> AppUserRoles { get; set; }

    public virtual DbSet<Employment> Employments { get; set; }

    public virtual DbSet<ExternalAccountLink> ExternalAccountLinks { get; set; }

    public virtual DbSet<ExternalSystem> ExternalSystems { get; set; }

    public virtual DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    public virtual DbSet<OrganizationUnitClosure> OrganizationUnitClosures { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => new { e.AuthType, e.AuthSubject }, "UX_Account_AuthType_Subject")
                .IsUnique()
                .HasFilter("([AuthSubject] IS NOT NULL)");

            entity.HasIndex(e => e.UserName, "UX_Account_UserName")
                .IsUnique()
                .HasFilter("([UserName] IS NOT NULL)");

            entity.Property(e => e.AccountId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AuthSubject).HasMaxLength(256);
            entity.Property(e => e.AuthType).HasMaxLength(32);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Person).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Person");
        });

        modelBuilder.Entity<AppPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);

            entity.ToTable("AppPermission");

            entity.HasIndex(e => e.Code, "UX_AppPermission_Code").IsUnique();

            entity.Property(e => e.PermissionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code).HasMaxLength(80);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("AppRole");

            entity.HasIndex(e => e.Code, "UX_AppRole_Code").IsUnique();

            entity.Property(e => e.RoleId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppRolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId });

            entity.ToTable("AppRolePermission");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Permission).WithMany(p => p.AppRolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARP_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.AppRolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ARP_Role");
        });

        modelBuilder.Entity<AppUserRole>(entity =>
        {
            entity.ToTable("AppUserRole");

            entity.HasIndex(e => new { e.AccountId, e.RoleId, e.OuId }, "UX_AppUserRole_Account_Role_Ou").IsUnique();

            entity.Property(e => e.AppUserRoleId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Account).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserRole_Account");

            entity.HasOne(d => d.Ou).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.OuId)
                .HasConstraintName("FK_AppUserRole_OU");

            entity.HasOne(d => d.Role).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUserRole_Role");
        });

        modelBuilder.Entity<Employment>(entity =>
        {
            entity.ToTable("Employment");

            entity.HasIndex(e => e.AccountId, "UX_Employment_Primary")
                .IsUnique()
                .HasFilter("([IsPrimary]=(1) AND [EndDate] IS NULL)");

            entity.Property(e => e.EmploymentId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EmployeeNo).HasMaxLength(50);
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(20)
                .HasDefaultValue("FullTime");
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Account).WithOne(p => p.Employment)
                .HasForeignKey<Employment>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employment_Account");

            entity.HasOne(d => d.Ou).WithMany(p => p.Employments)
                .HasForeignKey(d => d.OuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employment_OU");

            entity.HasOne(d => d.Position).WithMany(p => p.Employments)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_Employment_Position");
        });

        modelBuilder.Entity<ExternalAccountLink>(entity =>
        {
            entity.HasKey(e => e.LinkId);

            entity.ToTable("ExternalAccountLink");

            entity.HasIndex(e => new { e.SystemId, e.ExternalUserId }, "UX_EAL_System_ExternalUser").IsUnique();

            entity.Property(e => e.LinkId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ExternalRoleKey).HasMaxLength(128);
            entity.Property(e => e.ExternalUserId).HasMaxLength(128);
            entity.Property(e => e.ExternalUserName).HasMaxLength(128);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.SyncMessage).HasMaxLength(500);

            entity.HasOne(d => d.Account).WithMany(p => p.ExternalAccountLinks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EAL_Account");

            entity.HasOne(d => d.System).WithMany(p => p.ExternalAccountLinks)
                .HasForeignKey(d => d.SystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EAL_System");
        });

        modelBuilder.Entity<ExternalSystem>(entity =>
        {
            entity.HasKey(e => e.SystemId);

            entity.ToTable("ExternalSystem");

            entity.HasIndex(e => e.Code, "UX_ExternalSystem_Code").IsUnique();

            entity.Property(e => e.SystemId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AuthType).HasMaxLength(30);
            entity.Property(e => e.BaseUrl).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<OrganizationUnit>(entity =>
        {
            entity.HasKey(e => e.OuId);

            entity.ToTable("OrganizationUnit");

            entity.HasIndex(e => e.Code, "UX_OU_Code").IsUnique();

            entity.Property(e => e.OuId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<OrganizationUnitClosure>(entity =>
        {
            entity.HasKey(e => new { e.AncestorOuId, e.DescendantOuId });

            entity.ToTable("OrganizationUnitClosure");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AncestorOu).WithMany(p => p.OrganizationUnitClosureAncestorOus)
                .HasForeignKey(d => d.AncestorOuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OUC_Ancestor");

            entity.HasOne(d => d.DescendantOu).WithMany(p => p.OrganizationUnitClosureDescendantOus)
                .HasForeignKey(d => d.DescendantOuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OUC_Descendant");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.HasIndex(e => e.Email, "UX_Person_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.PersonId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Mobile).HasMaxLength(32);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.HasIndex(e => e.Code, "UX_Position_Code").IsUnique();

            entity.Property(e => e.PositionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
