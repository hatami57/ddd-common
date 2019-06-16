using System;
using System.Collections.Generic;
using DDDCommon.Domain.Interfaces;

namespace DDDCommon.Domain.Types
{
    public abstract class Entity<TId> : IEntity, IEquatable<Entity<TId>>
    {
        public virtual TId Id { get; protected set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity<TId> entity)
            {
                return Equals(entity);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IEquatable<Entity> Members

        public virtual bool Equals(Entity<TId> other)
        {
            return other != null && Id.Equals(other.Id);
        }

        #endregion
    }
}