using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyHelper
{
    public class TypeDefinition
    {
        public TypeInfo Type { get; private set; }

        public ICollection<TypeDefinition> SubTypes { get; }


        public TypeDefinition(TypeInfo type)
        {
            Type = type;
            SubTypes = new List<TypeDefinition>();
        }
    }

}
