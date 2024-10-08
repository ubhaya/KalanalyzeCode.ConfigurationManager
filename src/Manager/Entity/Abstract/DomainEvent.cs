﻿using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Entity.Abstract;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        DateOccurred = DateTimeOffset.UtcNow;
    }
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}