using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppUserRole
{
    public Guid AppUserRoleId { get; set; }

    public Guid AccountId { get; set; }

    public Guid RoleId { get; set; }

    public Guid? OuId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual OrganizationUnit? Ou { get; set; }

    public virtual AppRole Role { get; set; } = null!;
}
