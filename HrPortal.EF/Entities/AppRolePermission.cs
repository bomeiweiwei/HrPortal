using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppRolePermission
{
    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual AppPermission Permission { get; set; } = null!;

    public virtual AppRole Role { get; set; } = null!;
}
