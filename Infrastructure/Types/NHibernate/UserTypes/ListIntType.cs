using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes
{
    public class ListIntType : ListType<int>
    {
        public override SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.Object, 
                NpgsqlDbType.Array | NpgsqlDbType.Integer)};
    }
}