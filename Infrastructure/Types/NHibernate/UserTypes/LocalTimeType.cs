using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes
{
    public class LocalTimeType : IUserType
    {
        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(object x)
        {
            return x == null ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = rs[names[0]];
            return value == DBNull.Value ? null : value;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var param = (NpgsqlParameter)cmd.Parameters[index];
            param.NpgsqlDbType = NpgsqlDbType.Time;
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

        public SqlType[] SqlTypes => new SqlType[] {new NpgsqlExtendedSqlType(DbType.Time, NpgsqlDbType.Time)};
        public Type ReturnedType => typeof(LocalTime);
        public bool IsMutable => false;
    }
}