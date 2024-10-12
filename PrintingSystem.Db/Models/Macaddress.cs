using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class Macaddress
{
    public string Id { get; set; } = null!;

    public Guid PrintingDeviceId { get; set; }

    public virtual PrintingDevice PrintingDevice { get; set; } = null!;
}
