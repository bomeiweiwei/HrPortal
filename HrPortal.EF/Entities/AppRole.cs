using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppRole
{
    public Guid RoleId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

    public virtual ICollection<AppPermission> Permissions { get; set; } = new List<AppPermission>();
}
