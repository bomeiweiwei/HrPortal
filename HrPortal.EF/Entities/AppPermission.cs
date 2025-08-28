using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppPermission
{
    public Guid PermissionId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<AppRolePermission> AppRolePermissions { get; set; } = new List<AppRolePermission>();
}
