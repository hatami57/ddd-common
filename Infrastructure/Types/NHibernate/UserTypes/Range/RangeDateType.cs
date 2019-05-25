using System.Data;
using NHibernate.SqlTypes;
using NodaTime;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.Range
{
    public class RangeDateType : RangeType<LocalDate>
    {
        public override SqlType[] SqlTypes =>
            new SqlType[] {new NpgsqlExtendedSqlType(DbType.Object, 
                NpgsqlDbType.Range | NpgsqlDbType.Date)};
    }
}