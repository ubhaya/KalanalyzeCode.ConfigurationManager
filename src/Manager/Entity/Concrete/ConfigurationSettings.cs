﻿using KalanalyzeCode.ConfigurationManager.Entity.Abstract;

namespace KalanalyzeCode.ConfigurationManager.Entity.Concrete;

public class ConfigurationSettings : IEntity, IHasDomainEvent
{
    public string Id { get; set; } = string.Empty;
    public string? Value { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = [];
}