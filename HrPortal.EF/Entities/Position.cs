using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class Position
{
    public Guid PositionId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Employment> Employments { get; set; } = new List<Employment>();
}
