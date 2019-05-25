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
    public class JsonbType<T> : JsonType<T>
    {
        public override SqlType[] SqlTypes
        {
            get => new SqlType[] {new NpgsqlExtendedSqlType(DbType.String, NpgsqlDbType.Jsonb)};
        }
    }
}