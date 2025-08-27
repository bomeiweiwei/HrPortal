using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class OrganizationUnitClosure
{
    public Guid AncestorOuId { get; set; }

    public Guid DescendantOuId { get; set; }

    public int Depth { get; set; }

    public virtual OrganizationUnit AncestorOu { get; set; } = null!;

    public virtual OrganizationUnit DescendantOu { get; set; } = null!;
}
