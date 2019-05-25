using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List
{
    public class JsonType<T> : IUserType
    {
        public new bool Equals(object x, object y) => object.Equals(x, y);

        public int GetHashCode(object x) => x.GetHashCode();

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var value = rs[names[0]];
            return value == DBNull.Value || string.IsNullOrWhiteSpace((string) value)
                ? default
                : JsonConvert.DeserializeObject<T>((string) value);
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var param = (NpgsqlParameter) cmd.Parameters[index];
            param.NpgsqlValue = value == default ? (object)DBNull.Value : JsonConvert.SerializeObject(value);
        }

        public object DeepCopy(object value)
        {
            if (value == default) return default;
            
            var serialized = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public object Replace(object original, object target, object owner)
        {
            return Equals(original, target) ? original : DeepCopy(original);
        }

        public object Assemble(object cached, object owner)
        {
            return cached == default ? default : DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return value == default ? default : DeepCopy(value);
        }

        public virtual SqlType[] SqlTypes
        {
            get => new SqlType[] {new NpgsqlExtendedSqlType(DbType.String, NpgsqlDbType.Json)};
        }
        public Type ReturnedType => typeof(T);
        public bool IsMutable => true;
    }
}