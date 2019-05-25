using NHibernate.Spatial.Type;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes.Postgis
{
    public class Wgs84GeometryType : PostGisGeometryType
    {
        protected override void SetDefaultSRID(GeoAPI.Geometries.IGeometry geometry)
        {
            geometry.SRID = 4326;
        }
    }
}