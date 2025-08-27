using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class Person
{
    public Guid PersonId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public byte[] RowVer { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
