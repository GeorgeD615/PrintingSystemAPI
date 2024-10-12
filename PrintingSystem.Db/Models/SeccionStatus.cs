using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class SeccionStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
