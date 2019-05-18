using DDDCommon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDDCommon.Domain.Types
{
    public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    {
        public AggregateRoot(TId id) : base(id)
        {
        }
    }
}
