using System;
using System.Collections.Generic;

namespace PrintingSystem.Db.Models;

public partial class Office
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Installation? Installation { get; set; }
}
