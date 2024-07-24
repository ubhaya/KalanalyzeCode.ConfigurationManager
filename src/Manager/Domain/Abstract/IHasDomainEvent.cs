namespace KalanalyzeCode.ConfigurationManager.Domain.Abstract;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; set; }
}