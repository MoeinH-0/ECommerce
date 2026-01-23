namespace _0_Framework.Domain;

public abstract class EntityBase
{
    public long Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
}