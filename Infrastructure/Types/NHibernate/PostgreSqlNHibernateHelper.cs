using DDDCommon.Infrastructure.Types.NHibernate.Mappers;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using NHibernate.Tool.hbm2ddl;
using Npgsql;

namespace DDDCommon.Infrastructure.Types.NHibernate
{
    public class PostgreSqlNHibernateHelper
    {
        private readonly ISessionFactory _factory;

        public PostgreSqlNHibernateHelper(DbConfigurations dbConfigurations)
        {
            NpgsqlConnection.GlobalTypeMapper.UseNodaTime();
            NpgsqlConnection.GlobalTypeMapper.UseRawPostgis();

            var dbConfigurations1 = dbConfigurations;
            var cfg = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                    .Provider<global::NHibernate.Connection.DriverConnectionProvider>()
                    .Dialect<PostgreSQL83Dialect>()
                    .Dialect<global::NHibernate.Spatial.Dialect.PostGis20Dialect>()
                    .Driver<NpgsqlDriverExtended>()
                    .ConnectionString(dbConfigurations1.ConnectionString)
                )
                .Mappings(x =>
                {
                    dbConfigurations1.EntityTypeAssemblies.ForEach(y => x.FluentMappings.AddFromAssembly(y));
                    x.FluentMappings.Conventions.Add<NodaTimeConventions>();
                    x.FluentMappings.Conventions.Add<PostGisConventions>();
                    x.FluentMappings.Conventions.Add<ListConventions>();
                })
                .ExposeConfiguration(x => { new SchemaExport(x).Execute(false, true, false); })
                .BuildConfiguration();
            cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
            Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
            Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);
            _factory = cfg.BuildSessionFactory();
            var exp = new SchemaExport(cfg);
        }

        public ISession OpenSession() => _factory.OpenSession();
    }
}