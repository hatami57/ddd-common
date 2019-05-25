using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.NodaTime
{
    public class InstantType : IUserType
    {
        public new bool Equals(object x, object y) => object.Equals(x, y);

        public int GetHashCode(object x) => x == null ? 0 : x.GetHashCode();

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = rs[names[0]];
            return value == DBNull.Value ? null : value;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var param = (NpgsqlParameter)cmd.Parameters[index];
            param.NpgsqlValue = value ?? DBNull.Value;
        }

        public object DeepCopy(object value) => value;

        public object Replace(object original, object target, object owner) => original;

        public object Assemble(object cached, object owner) => cached;

        public object Disassemble(object value) => value;

        public SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.DateTime, NpgsqlDbType.Timestamp)};
        public Type ReturnedType => typeof(Instant);
        public bool IsMutable => false;
    }
}