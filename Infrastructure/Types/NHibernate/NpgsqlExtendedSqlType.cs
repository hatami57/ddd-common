using System.Data;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate
{
    public class NpgsqlExtendedSqlType : SqlType
    {
        public NpgsqlDbType NpgDbType { get; private set; }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType) : base(dbType)
        {
            NpgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length) : base(dbType, length)
        {
            NpgDbType = npgDbType;
        }

        public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale) : base(dbType,
            precision, scale)
        {
            NpgDbType = npgDbType;
        }
    }
}