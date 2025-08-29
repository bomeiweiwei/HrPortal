using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class OrganizationUnitClosure
{
    public Guid AncestorOuId { get; set; }

    public Guid DescendantOuId { get; set; }

    public int Depth { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
