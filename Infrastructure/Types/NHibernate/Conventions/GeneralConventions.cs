using DDDCommon.Infrastructure.Types.NHibernate.UserTypes.Range;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NodaTime;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.Conventions
{
    public class GeneralConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(string))
            {
                instance.CustomSqlType("text");
            }
        }
    }
}