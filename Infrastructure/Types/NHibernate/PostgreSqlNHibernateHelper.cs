using System.IO;
using DDDCommon.Infrastructure.Types.NHibernate.Conventions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Spatial.Mapping;
using NHibernate.Spatial.Metadata;
using NHibernate.Tool.hbm2ddl;
using Npgsql;

namespace DDDCommon.Infrastructure.Types.NHibernate
{
    public static class PostgreSqlSessionFactoryHelper
    {
        public static ISessionFactory Create(DbConfigurations dbConfigurations)
        {
            if (dbConfigurations.UseNodaTime) NpgsqlConnection.GlobalTypeMapper.UseNodaTime();

            var dbConfigurations1 = dbConfigurations;
            var persistenceCfg = PostgreSQLConfiguration.Standard
                .Provider<global::NHibernate.Connection.DriverConnectionProvider>()
                .Dialect<PostgreSQL83Dialect>()
                .Driver<NpgsqlDriverExtended>()
                .ConnectionString(dbConfigurations1.ConnectionString)
                .ShowSql();
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
                    x.FluentMappings.Conventions.Setup(y => y.Add(AutoImport.Never()));
                    if (dbConfigurations.UseNodaTime)
                        x.FluentMappings.Conventions.Add<NodaTimeConventions>();
                    if (dbConfigurations.UseNetTopologySuite)
                        x.FluentMappings.Conventions.Add<PostGisConventions>();
                    x.FluentMappings.Conventions.Add<GeneralConventions>();
                    x.FluentMappings.Conventions.Add<ListConventions>();
                    x.FluentMappings.Conventions.Add<RangeConventions>();
                    x.FluentMappings.Conventions.Add<JsonConventions>();
                })
                .ExposeConfiguration(x =>
                {
                    if (!string.IsNullOrWhiteSpace(dbConfigurations.SchemaExportFilename))
                        new SchemaExport(x).Execute(script =>
                                File.AppendAllText(dbConfigurations.SchemaExportFilename, script),
                            false, false);
                })
                .BuildConfiguration();

            if (dbConfigurations.UseNetTopologySuite)
            {
                cfg.AddAuxiliaryDatabaseObject(new SpatialAuxiliaryDatabaseObject(cfg));
                Metadata.AddMapping(cfg, MetadataClass.GeometryColumn);
                Metadata.AddMapping(cfg, MetadataClass.SpatialReferenceSystem);
            }

            return cfg.BuildSessionFactory();
        }
    }
}