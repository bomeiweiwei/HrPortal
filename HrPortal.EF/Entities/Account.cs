using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class Account
{
    public Guid AccountId { get; set; }

    public Guid? PersonId { get; set; }

    public string? UserName { get; set; }

    public string AuthType { get; set; } = null!;

    public string? AuthSubject { get; set; }

    public string? PasswordHash { get; set; }

    public byte Status { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public byte[] RowVer { get; set; } = null!;
}
