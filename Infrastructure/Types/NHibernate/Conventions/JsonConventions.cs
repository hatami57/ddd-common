using DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DDDCommon.Infrastructure.Types.NHibernate.Conventions
{
    public class JsonConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Type.IsGenericType && instance.Type.GetGenericTypeDefinition() == typeof(JsonType<>))
                instance.CustomSqlType("json");
            else if (instance.Type.IsGenericType && instance.Type.GetGenericTypeDefinition() == typeof(JsonbType<>))
                instance.CustomSqlType("jsonb");
        }
    }
}