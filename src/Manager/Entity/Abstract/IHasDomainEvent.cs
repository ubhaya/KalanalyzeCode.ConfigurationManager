namespace KalanalyzeCode.ConfigurationManager.Entity.Abstract;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; set; }
}