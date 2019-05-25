using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List
{
    public class ListStringType : ReadOnlyListType<string>
    {
        public override SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.Object, 
                NpgsqlDbType.Array | NpgsqlDbType.Text)};

        protected override object DeepCopyNotNull(object value) => new List<string>((IReadOnlyList<string>) value);
    }
}