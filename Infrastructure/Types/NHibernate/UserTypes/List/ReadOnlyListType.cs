using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List
{
    public abstract class ReadOnlyListType<T> : IUserType
    {
        public new bool Equals(object x, object y)
        {
            var xList = x as IReadOnlyList<T>;
            var yList = y as IReadOnlyList<T>;
            if (xList == null && yList == null) return true;
            if (xList == null || yList == null) return false;
            if (xList.Count != yList.Count) return false;
            for (var i = 0; i < xList.Count; i++)
            {
                if (!xList[i].Equals(yList[i])) return false;
            }

            return true;
        }

        public int GetHashCode(object x)
        {
            unchecked
            {
                var hash = 19;
                foreach (var item in (IReadOnlyList<T>) x)
                {
                    hash = hash * 31 + item.GetHashCode();
                }

                return hash;
            }
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = rs[names[0]];
            return value == DBNull.Value ? null : ((T[]) value).ToList();
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var param = (NpgsqlParameter) cmd.Parameters[index];
            param.NpgsqlValue = value ?? DBNull.Value;
        }

        public object DeepCopy(object value) => value == null ? null : DeepCopyNotNull(value);

        public object Replace(object original, object target, object owner)
        {
            return Equals(original, target) ? original : DeepCopy(original);
        }

        public object Assemble(object cached, object owner)
        {
            return cached == null ? null : DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return value == null ? null : DeepCopy(value);
        }

        public Type ReturnedType => typeof(List<T>);
        public bool IsMutable => true;
        
        public abstract SqlType[] SqlTypes { get; }
        protected abstract object DeepCopyNotNull(object value);
    }
}