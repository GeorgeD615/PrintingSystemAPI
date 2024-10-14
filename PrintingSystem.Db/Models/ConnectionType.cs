namespace PrintingSystem.Db.Models;

public partial class ConnectionType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PrintingDevice> PrintingDevices { get; set; } = new List<PrintingDevice>();
}
