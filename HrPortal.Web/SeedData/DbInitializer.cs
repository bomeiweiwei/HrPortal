using System;
using HrPortal.EF.Data;
using HrPortal.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HrPortal.Web.SeedData
{
    public static class DbInitializer
    {
        public static void Seed(PersonSystemContext context)
        {
            context.Database.Migrate();

            // 如果已有帳號，代表種子資料跑過 → 不重複執行
            if (context.Accounts.Any())
                return;

            var hasher = new PasswordHasher<Account>();

            // 固定 GUID
            var personWang = Guid.Parse("30000000-0000-0000-0000-000000000001");
            var personLi = Guid.Parse("30000000-0000-0000-0000-000000000002");
            var personChen = Guid.Parse("30000000-0000-0000-0000-000000000003");

            var accWang = Guid.Parse("40000000-0000-0000-0000-000000000001");
            var accLi = Guid.Parse("40000000-0000-0000-0000-000000000002");
            var accChen = Guid.Parse("40000000-0000-0000-0000-000000000003");

            var ouGroup = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var ouRND = Guid.Parse("10000000-0000-0000-0000-000000000006");

            var posMgr = Guid.Parse("20000000-0000-0000-0000-000000000001");
            var posEng = Guid.Parse("20000000-0000-0000-0000-000000000002");

            var roleAdmin = Guid.Parse("70000000-0000-0000-0000-000000000001");
            var roleEditor = Guid.Parse("70000000-0000-0000-0000-000000000002");
            var roleViewer = Guid.Parse("70000000-0000-0000-0000-000000000003");

            var permPersonRead = Guid.Parse("80000000-0000-0000-0000-000000000001");
            var permPersonWrite = Guid.Parse("80000000-0000-0000-0000-000000000002");

            // === Admin 帳號 (建立人 Id) ===
            var adminAccountId = accWang;

            // === Person ===
            context.People.AddRange(
                new Person { PersonId = personWang, FullName = "王大明", Email = "daming.wang@example.com", Mobile = "0912-000-001", CreatedBy = adminAccountId },
                new Person { PersonId = personLi, FullName = "李小安", Email = "an.li@example.com", Mobile = "0912-000-002", CreatedBy = adminAccountId },
                new Person { PersonId = personChen, FullName = "陳冠廷", Email = "ktchen@example.com", Mobile = "0912-000-003", CreatedBy = adminAccountId }
            );

            // === Account (密碼:1234) ===
            var tmpAcc = new Account();
            context.Accounts.AddRange(
                new Account { AccountId = accWang, PersonId = personWang, UserName = "dwang", AuthType = "Local", PasswordHash = hasher.HashPassword(tmpAcc, "1234"), Status = 1, CreatedBy = adminAccountId },
                new Account { AccountId = accLi, PersonId = personLi, UserName = "ali", AuthType = "Local", PasswordHash = hasher.HashPassword(tmpAcc, "1234"), Status = 1, CreatedBy = adminAccountId },
                new Account { AccountId = accChen, PersonId = personChen, UserName = "ktchen", AuthType = "Local", PasswordHash = hasher.HashPassword(tmpAcc, "1234"), Status = 1, CreatedBy = adminAccountId }
            );

            // === OrganizationUnit ===
            context.OrganizationUnits.AddRange(
                new OrganizationUnit { OuId = ouGroup, Code = "GROUP", Name = "總公司", Type = "Group", CreatedBy = adminAccountId },
                new OrganizationUnit { OuId = ouRND, Code = "RND", Name = "研發部", Type = "Dept", CreatedBy = adminAccountId }
            );

            // === Position ===
            context.Positions.AddRange(
                new Position { PositionId = posMgr, Code = "RND_MGR", Name = "研發主管", CreatedBy = adminAccountId },
                new Position { PositionId = posEng, Code = "ENG", Name = "工程師", CreatedBy = adminAccountId }
            );

            // === Employment ===
            context.Employments.AddRange(
                new Employment { EmploymentId = Guid.NewGuid(), AccountId = accWang, OuId = ouRND, PositionId = posMgr, EmployeeNo = "RD0001", IsPrimary = true, StartDate = new DateTime(2024, 1, 1), CreatedBy = adminAccountId },
                new Employment { EmploymentId = Guid.NewGuid(), AccountId = accLi, OuId = ouRND, PositionId = posEng, EmployeeNo = "RD0002", IsPrimary = true, StartDate = new DateTime(2024, 3, 15), CreatedBy = adminAccountId },
                new Employment { EmploymentId = Guid.NewGuid(), AccountId = accChen, OuId = ouRND, PositionId = posEng, EmployeeNo = "RD0003", IsPrimary = true, StartDate = new DateTime(2024, 7, 1), CreatedBy = adminAccountId }
            );

            // === AppRole ===
            context.AppRoles.AddRange(
                new AppRole { RoleId = roleAdmin, Code = "HR_ADMIN", Name = "人事管理員", CreatedBy = adminAccountId },
                new AppRole { RoleId = roleEditor, Code = "HR_EDITOR", Name = "人事編輯員", CreatedBy = adminAccountId },
                new AppRole { RoleId = roleViewer, Code = "HR_VIEWER", Name = "人事檢視員", CreatedBy = adminAccountId }
            );

            // === AppPermission ===
            context.AppPermissions.AddRange(
                new AppPermission { PermissionId = permPersonRead, Code = "PERSON.READ", Name = "讀取人員資料", CreatedBy = adminAccountId },
                new AppPermission { PermissionId = permPersonWrite, Code = "PERSON.WRITE", Name = "維護人員資料", CreatedBy = adminAccountId }
            );

            // === AppRolePermission ===
            context.AppRolePermissions.AddRange(
                new AppRolePermission { RoleId = roleAdmin, PermissionId = permPersonRead, CreatedBy = adminAccountId },
                new AppRolePermission { RoleId = roleAdmin, PermissionId = permPersonWrite, CreatedBy = adminAccountId },
                new AppRolePermission { RoleId = roleEditor, PermissionId = permPersonRead, CreatedBy = adminAccountId },
                new AppRolePermission { RoleId = roleEditor, PermissionId = permPersonWrite, CreatedBy = adminAccountId },
                new AppRolePermission { RoleId = roleViewer, PermissionId = permPersonRead, CreatedBy = adminAccountId }
            );

            // === AppUserRole ===
            context.AppUserRoles.AddRange(
                new AppUserRole { AppUserRoleId = Guid.NewGuid(), AccountId = accWang, RoleId = roleAdmin, OuId = null, CreatedBy = adminAccountId },
                new AppUserRole { AppUserRoleId = Guid.NewGuid(), AccountId = accLi, RoleId = roleEditor, OuId = ouRND, CreatedBy = adminAccountId },
                new AppUserRole { AppUserRoleId = Guid.NewGuid(), AccountId = accChen, RoleId = roleViewer, OuId = ouRND, CreatedBy = adminAccountId }
            );

            context.SaveChanges();
        }
    }
}

