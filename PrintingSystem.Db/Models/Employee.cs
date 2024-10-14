namespace PrintingSystem.Db.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public Guid OfficeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Office Office { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
