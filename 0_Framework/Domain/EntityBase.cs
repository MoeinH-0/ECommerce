namespace _0_Framework.Domain;

public abstract class EntityBase
{
    public long Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}