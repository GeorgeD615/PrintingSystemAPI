namespace PrintingSystem.Db.Models;

public partial class Office
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Installation> Installations { get; set; } = new List<Installation>();
}
