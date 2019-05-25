using DDDCommon.Infrastructure.Types.NHibernate.UserTypes.Range;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NodaTime;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.Conventions
{
    public class RangeConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(NpgsqlRange<LocalDate>))
            {
                instance.CustomType<RangeDateType>();
                instance.CustomSqlType("daterange");
            }
        }
    }
}