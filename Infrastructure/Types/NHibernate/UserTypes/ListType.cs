using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes
{
    public abstract class ListType<T> : IUserType
    {
        public new bool Equals(object x, object y)
        {
            var xList = x as List<T>;
            var yList = y as List<T>;
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
                foreach (var item in (IList) x)
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

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public abstract SqlType[] SqlTypes { get; }

        public Type ReturnedType => typeof(List<T>);
        public bool IsMutable => false;
    }
}