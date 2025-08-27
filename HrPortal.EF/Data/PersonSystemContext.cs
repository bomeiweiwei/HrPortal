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
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6403FFE45");

            entity.ToTable("Account");

            entity.HasIndex(e => new { e.AuthType, e.AuthSubject }, "UX_Account_Auth").IsUnique();

            entity.HasIndex(e => e.UserName, "UX_Account_UserName").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.AuthSubject).HasMaxLength(256);
            entity.Property(e => e.AuthType).HasMaxLength(32);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Person).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__PersonI__3E52440B");
        });

        modelBuilder.Entity<AppPermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__AppPermi__EFA6FB2F50DA0C19");

            entity.ToTable("AppPermission");

            entity.HasIndex(e => e.Code, "UQ__AppPermi__A25C5AA72FEC5AC0").IsUnique();

            entity.Property(e => e.PermissionId).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(80);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AppRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__AppRole__8AFACE1A64811567");

            entity.ToTable("AppRole");

            entity.HasIndex(e => e.Code, "UQ__AppRole__A25C5AA752406302").IsUnique();

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "AppRolePermission",
                    r => r.HasOne<AppPermission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AppRolePe__Permi__7F2BE32F"),
                    l => l.HasOne<AppRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AppRolePe__RoleI__7E37BEF6"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId");
                        j.ToTable("AppRolePermission");
                    });
        });

        modelBuilder.Entity<AppUserRole>(entity =>
        {
            entity.HasKey(e => e.AppUserRoleId).HasName("PK__AppUserR__BD9D3D2906348351");

            entity.ToTable("AppUserRole");

            entity.HasIndex(e => new { e.AccountId, e.RoleId, e.OuId }, "UX_AppUserRole_Unique").IsUnique();

            entity.Property(e => e.AppUserRoleId).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUserRo__Accou__797309D9");

            entity.HasOne(d => d.Ou).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.OuId)
                .HasConstraintName("FK__AppUserRol__OuId__7B5B524B");

            entity.HasOne(d => d.Role).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUserRo__RoleI__7A672E12");
        });

        modelBuilder.Entity<Employment>(entity =>
        {
            entity.HasKey(e => e.EmploymentId).HasName("PK__Employme__FDC872B67E7F8551");

            entity.ToTable("Employment");

            entity.HasIndex(e => e.AccountId, "UX_Employment_Primary").IsUnique();

            entity.Property(e => e.EmploymentId).ValueGeneratedNever();
            entity.Property(e => e.EmployeeNo).HasMaxLength(50);
            entity.Property(e => e.EmploymentType).HasMaxLength(20);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Account).WithOne(p => p.Employment)
                .HasForeignKey<Employment>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employmen__Accou__5165187F");

            entity.HasOne(d => d.Ou).WithMany(p => p.Employments)
                .HasForeignKey(d => d.OuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employment__OuId__52593CB8");

            entity.HasOne(d => d.Position).WithMany(p => p.Employments)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Employmen__Posit__534D60F1");
        });

        modelBuilder.Entity<ExternalAccountLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__External__2D1221357A65A19B");

            entity.ToTable("ExternalAccountLink");

            entity.HasIndex(e => new { e.SystemId, e.ExternalUserId }, "UX_ExternalLink").IsUnique();

            entity.Property(e => e.LinkId).ValueGeneratedNever();
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
                .HasConstraintName("FK__ExternalA__Accou__5EBF139D");

            entity.HasOne(d => d.System).WithMany(p => p.ExternalAccountLinks)
                .HasForeignKey(d => d.SystemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExternalA__Syste__5FB337D6");
        });

        modelBuilder.Entity<ExternalSystem>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PK__External__9394F68A22B2DC61");

            entity.ToTable("ExternalSystem");

            entity.HasIndex(e => e.Code, "UX_ExternalSystem_Code").IsUnique();

            entity.Property(e => e.SystemId).ValueGeneratedNever();
            entity.Property(e => e.AuthType).HasMaxLength(30);
            entity.Property(e => e.BaseUrl).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<OrganizationUnit>(entity =>
        {
            entity.HasKey(e => e.OuId).HasName("PK__Organiza__FC76A228D6EF9B4D");

            entity.ToTable("OrganizationUnit");

            entity.HasIndex(e => e.Code, "UX_OU_Code").IsUnique();

            entity.Property(e => e.OuId).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<OrganizationUnitClosure>(entity =>
        {
            entity.HasKey(e => new { e.AncestorOuId, e.DescendantOuId }).HasName("PK_OU_Closure");

            entity.ToTable("OrganizationUnitClosure");

            entity.HasOne(d => d.AncestorOu).WithMany(p => p.OrganizationUnitClosureAncestorOus)
                .HasForeignKey(d => d.AncestorOuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Organizat__Ances__49C3F6B7");

            entity.HasOne(d => d.DescendantOu).WithMany(p => p.OrganizationUnitClosureDescendantOus)
                .HasForeignKey(d => d.DescendantOuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Organizat__Desce__4AB81AF0");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFBE51B715D32");

            entity.ToTable("Person");

            entity.HasIndex(e => e.Email, "UX_Person_Email").IsUnique();

            entity.Property(e => e.PersonId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(64);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(32);
            entity.Property(e => e.RowVer)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedBy).HasMaxLength(64);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A7931A445FF");

            entity.ToTable("Position");

            entity.HasIndex(e => e.Code, "UX_Position_Code").IsUnique();

            entity.Property(e => e.PositionId).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
