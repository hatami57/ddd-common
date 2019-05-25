using DDDCommon.Infrastructure.Types.NHibernate.UserTypes.Postgis;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NetTopologySuite.Geometries;

namespace DDDCommon.Infrastructure.Types.NHibernate.Conventions
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