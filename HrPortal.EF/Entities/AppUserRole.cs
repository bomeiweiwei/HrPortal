using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class AppUserRole
{
    public Guid AppUserRoleId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? OuId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
