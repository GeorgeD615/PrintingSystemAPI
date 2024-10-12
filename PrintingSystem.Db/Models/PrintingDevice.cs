using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class PrintingDevice
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? ConnectionTypeId { get; set; }

    public virtual ConnectionType? ConnectionType { get; set; }

    public virtual ICollection<Installation> Installations { get; set; } = new List<Installation>();

    public virtual ICollection<Macaddress> Macaddresses { get; set; } = new List<Macaddress>();
}
