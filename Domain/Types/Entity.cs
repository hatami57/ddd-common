using System;
using System.Collections.Generic;

namespace DDDCommon.Domain.Types
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        public TId Id { get; protected set; }

        protected Entity(TId id)
        {
            //if (object.Equals(id, default(TId)))
            //{
            //    throw new ArgumentException("The ID cannot be the default value.", nameof(id));
            //}

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

        public bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }
            return Id.Equals(other.Id);
        }

        #endregion
    }
}