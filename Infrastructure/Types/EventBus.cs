using DDDCommon.Domain.Interfaces;
using DDDCommon.Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDCommon.Infrastructure.Types
{
    public class EventBus : IEventBus
    {
        private readonly Queue<DomainEvent> _bus = new Queue<DomainEvent>();
        public int Count => _bus.Count;

        public void Dispatch(DomainEvent @event)
        {
            _bus.Enqueue(@event);
        }

        public DomainEvent DequeueEvent() => _bus.Dequeue();
    }

}
