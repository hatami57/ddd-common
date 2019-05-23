using System;
using DDDCommon.Infrastructure.Types.NHibernate.UserTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NetTopologySuite.Geometries;
using NodaTime;

namespace DDDCommon.Infrastructure.Types.NHibernate.Mappers
{
    public class PostGisConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (!instance.Property.PropertyType.IsSubclassOf(typeof(Geometry))) return;
            
            instance.CustomType<Wgs84GeometryType>();
            instance.CustomSqlType("geometry");
        }
    }
}