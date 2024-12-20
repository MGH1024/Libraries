﻿using MGH.Core.Domain.BaseEntity.Abstract.Events;

namespace MGH.Core.Infrastructure.MessageBroker;

public interface IEventBusDispatcher 
{
    void Publish<T>(T model) where T : IEvent ;
    void Publish<T>(IEnumerable<T> models) where T : IEvent;
}