using DDDCommon.Domain.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DDDCommon.Domain.Interfaces
{
    public interface IEventBus
    {
        void Dispatch(DomainEvent notification);
    }
}
