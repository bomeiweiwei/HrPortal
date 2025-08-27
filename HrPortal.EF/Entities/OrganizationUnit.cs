using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class OrganizationUnit
{
    public Guid OuId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVer { get; set; } = null!;

    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

    public virtual ICollection<Employment> Employments { get; set; } = new List<Employment>();

    public virtual ICollection<OrganizationUnitClosure> OrganizationUnitClosureAncestorOus { get; set; } = new List<OrganizationUnitClosure>();

    public virtual ICollection<OrganizationUnitClosure> OrganizationUnitClosureDescendantOus { get; set; } = new List<OrganizationUnitClosure>();
}
