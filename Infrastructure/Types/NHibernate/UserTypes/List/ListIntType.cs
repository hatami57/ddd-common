using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List
{
    public class ListIntType : ReadOnlyListType<int>
    {
        public override SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.Object, 
                NpgsqlDbType.Array | NpgsqlDbType.Integer)};

        protected override object DeepCopyNotNull(object value) => new List<int>((IReadOnlyList<int>) value);
    }
}