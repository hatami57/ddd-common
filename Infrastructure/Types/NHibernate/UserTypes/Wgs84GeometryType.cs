using System.Data;
using NHibernate.Spatial.Type;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace DDDCommon.Infrastructure.Types.NHibernate.UserTypes
{
    public class Wgs84GeometryType : PostGisGeometryType
    {
        protected override void SetDefaultSRID(GeoAPI.Geometries.IGeometry geometry)
        {
            geometry.SRID = 4326;
        }
    }
}