using System;
using DDDCommon.Infrastructure.Types.NHibernate.UserTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NodaTime;

namespace DDDCommon.Infrastructure.Types.NHibernate.Mappers
{
    public class NodaTimeConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(LocalTime) ||
                instance.Property.PropertyType == typeof(LocalTime?))
                instance.CustomType<LocalTimeType>();
            else if (instance.Property.PropertyType == typeof(LocalDate) ||
                     instance.Property.PropertyType == typeof(LocalDate?))
                instance.CustomType<LocalDateType>();
            else if (instance.Property.PropertyType == typeof(Instant) ||
                     instance.Property.PropertyType == typeof(Instant?))
                instance.CustomType<InstantType>();
        }
    }
}