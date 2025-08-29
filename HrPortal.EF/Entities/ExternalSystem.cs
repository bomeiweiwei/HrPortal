using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class ExternalSystem
{
    public Guid SystemId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? BaseUrl { get; set; }

    public string AuthType { get; set; } = null!;

    public string? Metadata { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
