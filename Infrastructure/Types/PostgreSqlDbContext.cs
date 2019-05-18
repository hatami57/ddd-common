using DDDCommon.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Npgsql.NameTranslation;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCommon.Infrastructure.Types
{
    public class PostgreSqlDbContext : DbContext
    {
        private static readonly Regex KeysRegex = new Regex("^(PK|FK|IX)_", RegexOptions.Compiled);
        private readonly EventBus _bus;
        private readonly IMediator _mediator;
        private readonly DbConfigurations _dbConfigurations;
        //[Obsolete]
        //public static readonly LoggerFactory MyLoggerFactory
        //    = new LoggerFactory(new[] { new ConsoleLoggerProvider((category, level)
        //        => category == DbLoggerCategory.Database.Command.Name, true) });

        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options,
            EventBus bus, IMediator mediator, DbConfigurations dbConfigurations)
            : base(options)
        {
            _bus = bus;
            _mediator = mediator;
            _dbConfigurations = dbConfigurations;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder
                //.UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseNpgsql(_dbConfigurations.ConnectionString, o =>
                {
                    if (_dbConfigurations.UseNodaTime)
                        o.UseNodaTime();

                    if (_dbConfigurations.UseNetTopologySuite)
                        o.UseNetTopologySuite();
                });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ForNpgsqlUseIdentityByDefaultColumns()
                .HasPostgresExtension("citext");

            if (_dbConfigurations.UseNetTopologySuite)
                modelBuilder.HasPostgresExtension("postgis");

            _dbConfigurations.EntityTypeAssemblies.ForEach(x => modelBuilder.ApplyConfigurationsFromAssembly(x));

            FixSnakeCaseNames(modelBuilder);

            Console.WriteLine("\n\nOn Model Creating!\n\n");
        }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
            if (!(Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator))
                return;
            var schemas = $"'{string.Join("', '", _dbConfigurations.Schemas)}'";
            var sql = "SELECT count(*) FROM information_schema.tables " +
                $"WHERE table_schema IN ({schemas}) AND table_type = 'BASE TABLE' AND table_name NOT IN('__EFMigrationsHistory', 'spatial_ref_sys');";
            Database.OpenConnection();
            using (var cmd = Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sql;
                if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    databaseCreator.CreateTables();
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            while (_bus.Count > 0)
            {
                var @event = _bus.DequeueEvent();
                await _mediator.Publish(@event, cancellationToken);
            }
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException e)
            {
                if (!(e.InnerException is PostgresException pgError))
                    throw Errors.InternalError(e.InnerException != null
                        ? e.InnerException.Message
                        : e.Message);

                switch (pgError.SqlState)
                {
                    case "23505":
                        throw Errors.DuplicateKey(pgError.Detail);
                    default:
                        throw Errors.DatabaseError(pgError.Detail);
                }
            }
            catch (Exception e)
            {
                throw Errors.InternalError(e.InnerException != null
                    ? e.InnerException.Message
                    : e.Message);
            }
        }

        private void FixSnakeCaseNames(ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var table in modelBuilder.Model.GetEntityTypes())
            {
                ConvertToSnake(mapper, table);
                foreach (var property in table.GetProperties())
                {
                    ConvertToSnake(mapper, property);
                }

                foreach (var primaryKey in table.GetKeys())
                {
                    ConvertToSnake(mapper, primaryKey);
                }

                foreach (var foreignKey in table.GetForeignKeys())
                {
                    ConvertToSnake(mapper, foreignKey);
                }

                foreach (var indexKey in table.GetIndexes())
                {
                    ConvertToSnake(mapper, indexKey);
                }
            }
        }

        private void ConvertToSnake(INpgsqlNameTranslator mapper, object entity)
        {
            switch (entity)
            {
                case IMutableEntityType table:
                    var relationalTable = table.Relational();
                    relationalTable.TableName = ConvertGeneralToSnake(mapper, relationalTable.TableName);
                    if (relationalTable.TableName.StartsWith("asp_net_"))
                    {
                        relationalTable.TableName = relationalTable.TableName.Replace("asp_net_", string.Empty);
                        relationalTable.Schema = "identity";
                    }

                    break;
                case IMutableProperty property:
                    property.Relational().ColumnName = ConvertGeneralToSnake(mapper, property.Relational().ColumnName);
                    break;
                case IMutableKey primaryKey:
                    primaryKey.Relational().Name = ConvertKeyToSnake(mapper, primaryKey.Relational().Name);
                    break;
                case IMutableForeignKey foreignKey:
                    foreignKey.Relational().Name = ConvertKeyToSnake(mapper, foreignKey.Relational().Name);
                    break;
                case IMutableIndex indexKey:
                    indexKey.Relational().Name = ConvertKeyToSnake(mapper, indexKey.Relational().Name);
                    break;
                default:
                    throw new NotImplementedException("Unexpected type was provided to snake case converter");
            }
        }

        private string ConvertKeyToSnake(INpgsqlNameTranslator mapper, string keyName) =>
            ConvertGeneralToSnake(mapper, KeysRegex.Replace(keyName, 
                match => match.Value.ToLower()));

        private string ConvertGeneralToSnake(INpgsqlNameTranslator mapper, string entityName) =>
            mapper.TranslateMemberName(ModifyNameBeforeConversion(mapper, entityName));

        protected virtual string ModifyNameBeforeConversion(INpgsqlNameTranslator mapper, string entityName) =>
            entityName;

    }
}
