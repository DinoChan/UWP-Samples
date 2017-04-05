using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;

namespace AssemblyHelper
{
   public static class TypeSeeker
    {
        public static TypeDefinition SeekType(Type type)
        {
            TypeDefinition result = new TypeDefinition(type.GetTypeInfo());
            foreach (var item in type.GetTypeInfo().Assembly.ExportedTypes.Where(t => t.GetTypeInfo().BaseType == type))
            {
                var subType = SeekType(item);
                result.SubTypes.Add(subType);
            }
            return result;
        }
    }
}
