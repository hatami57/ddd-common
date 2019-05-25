using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List
{
    public class ListShortType : ReadOnlyListType<short>
    {
        public override SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.Object, 
                NpgsqlDbType.Array | NpgsqlDbType.Smallint)};

        protected override object DeepCopyNotNull(object value) => new List<short>((IReadOnlyList<short>) value);
    }
}