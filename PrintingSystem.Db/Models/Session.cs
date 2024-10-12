using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class Session
{
    public Guid Id { get; set; }

    public string TaskName { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public Guid InstallationId { get; set; }

    public int NumberOfPages { get; set; }

    public Guid StatusId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Installation Installation { get; set; } = null!;

    public virtual SeccionStatus Status { get; set; } = null!;
}
