using DDDCommon.Infrastructure.Types.NHibernate.Conventions;
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
    public class PostgreSqlSessionFactoryHelper
    {
        public ISessionFactory SessionFactory { get; }

        public PostgreSqlSessionFactoryHelper(DbConfigurations dbConfigurations)
        {
            if (dbConfigurations.UseNodaTime) NpgsqlConnection.GlobalTypeMapper.UseNodaTime();

            var dbConfigurations1 = dbConfigurations;
            var persistenceCfg = PostgreSQLConfiguration.Standard
                .Provider<global::NHibernate.Connection.DriverConnectionProvider>()
                .Dialect<PostgreSQL83Dialect>()
                .Driver<NpgsqlDriverExtended>()
                .ConnectionString(dbConfigurations1.ConnectionString);
            if (dbConfigurations.UseNetTopologySuite)
            {
                NpgsqlConnection.GlobalTypeMapper.UseRawPostgis();
                persistenceCfg.Dialect<global::NHibernate.Spatial.Dialect.PostGis20Dialect>();
            }
            var cfg = Fluently.Configure()
                .Database(persistenceCfg)
                .Mappings(x =>
                {
                    dbConfigurations1.EntityTypeAssemblies.ForEach(y => x.FluentMappings.AddFromAssembly(y));
                    x.FluentMappings.Conventions.Add<GeneralConventions>();
                    x.FluentMappings.Conventions.Add<NodaTimeConventions>();
                    x.FluentMappings.Conventions.Add<PostGisConventions>();
                    x.FluentMappings.Conventions.Add<ListConventions>();
                    x.FluentMappings.Conventions.Add<RangeConventions>();
                    x.FluentMappings.Conventions.Add<JsonConventions>();
                })
                .ExposeConfiguration(x => { new SchemaExport(x).Execute(false, true, false); })
                .BuildConfiguration();
            cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
            Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
            Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);
            SessionFactory = cfg.BuildSessionFactory();
        }
    }
}