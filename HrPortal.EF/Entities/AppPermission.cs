using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppPermission
{
    public Guid PermissionId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
