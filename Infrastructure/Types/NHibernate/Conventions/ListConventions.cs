using System;
using System.Collections.Generic;
using System.Linq;
using DDDCommon.Infrastructure.Types.NHibernate.UserTypes.List;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DDDCommon.Infrastructure.Types.NHibernate.Conventions
{
    public class ListConventions : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (!SetCustomType(instance, instance.Property.PropertyType))
            {
                instance.Property.PropertyType.GetInterfaces().Any(x => SetCustomType(instance, x));
            }
        }

        private static bool SetCustomType(IPropertyInstance instance, Type type)
        {
            if (type == typeof(IReadOnlyList<int>))
            {
                instance.CustomType<ListIntType>();
                instance.CustomSqlType("integer[]");
                return true;
            }
            if (type == typeof(IReadOnlyList<short>))
            {
                instance.CustomType<ListShortType>();
                instance.CustomSqlType("smallint[]");
                return true;
            }
            if (type == typeof(IReadOnlyList<string>))
            {
                instance.CustomType<ListStringType>();
                instance.CustomSqlType("text[]");
                return true;
            }

            return false;
        }
    }
}