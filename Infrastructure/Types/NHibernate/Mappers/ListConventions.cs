using System;
using System.Collections.Generic;
using DDDCommon.Infrastructure.Types.NHibernate.UserTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NetTopologySuite.Geometries;
using NodaTime;

namespace DDDCommon.Infrastructure.Types.NHibernate.Mappers
{
    public class ListConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(List<int>))
            {
                instance.CustomType<ListIntType>();
                instance.CustomSqlType("integer[]");
            }
        }
    }
}