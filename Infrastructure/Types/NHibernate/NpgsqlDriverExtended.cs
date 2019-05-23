using System.Data.Common;
using NHibernate;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;

namespace DDDCommon.Infrastructure.Types.NHibernate
{
    public class NpgsqlDriverExtended : NpgsqlDriver
    {
        protected override void InitializeParameter(DbParameter dbParameter, string name, SqlType sqlType)
        {
            if (sqlType is NpgsqlExtendedSqlType type && dbParameter is NpgsqlParameter parameter)
            {
                InitializeParameter(parameter, name, type);
            }
            else
            {
                base.InitializeParameter(dbParameter, name, sqlType);
            }
        }

        protected virtual void InitializeParameter(NpgsqlParameter dbParam, string name, NpgsqlExtendedSqlType sqlType)
        {
            if (sqlType == null)
                throw new QueryException($"No type assigned to parameter '{name}'");

            dbParam.ParameterName = FormatNameForParameter(name);
            dbParam.DbType = sqlType.DbType;
            dbParam.NpgsqlDbType = sqlType.NpgDbType;

        }
    }
}