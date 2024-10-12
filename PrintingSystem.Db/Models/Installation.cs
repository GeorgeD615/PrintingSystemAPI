using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class Installation
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int? InstallationOrderNumber { get; set; }

    public bool IsDefault { get; set; }

    public Guid PrintingDeviceId { get; set; }

    public Guid OfficeId { get; set; }

    public virtual Office Office { get; set; } = null!;

    public virtual PrintingDevice PrintingDevice { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
