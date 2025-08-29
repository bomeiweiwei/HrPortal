using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class Employment
{
    public Guid EmploymentId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? OuId { get; set; }

    public Guid? PositionId { get; set; }

    public string? EmployeeNo { get; set; }

    public string EmploymentType { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public byte[] RowVer { get; set; } = null!;
}
