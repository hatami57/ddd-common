using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DDDCommon.Infrastructure.Types
{
    public class DbConfigurations
    {
        public List<Assembly> EntityTypeAssemblies { get; set; } = new List<Assembly>();
        public List<string> Schemas { get; set; } = new List<string>();
        public string ConnectionString { get; set; }
        public bool UseNodaTime { get; set; }
        public bool UseNetTopologySuite { get; set; }
    }
}
