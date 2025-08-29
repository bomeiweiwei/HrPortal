using System;
using System.Collections.Generic;

namespace HrPortal.EF.Entities;

public partial class ExternalAccountLink
{
    public Guid LinkId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? SystemId { get; set; }

    public string ExternalUserId { get; set; } = null!;

    public string? ExternalUserName { get; set; }

    public string? ExternalRoleKey { get; set; }

    public DateTime? LastSyncAt { get; set; }

    public byte? SyncStatus { get; set; }

    public string? SyncMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public byte[] RowVer { get; set; } = null!;
}
