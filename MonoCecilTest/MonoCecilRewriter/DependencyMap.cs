using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter
{
    public class DependencyMap
    {
        private readonly Dictionary<string, Dependency> map = new Dictionary<string, Dependency>();
        
        public void Add(string source, string target, PropertyType property_type)
        {
            if (!map.ContainsKey(source))
                map.Add(source, new Dependency(source, target, property_type));

            map[source].targets.Add(target);
        }

        public IEnumerable<string> GetDependenciesFor(string source)
        {
            return (map.ContainsKey(source) ? map[source].targets : new List<string>());
        }

        public void Dump()
        {
            Console.WriteLine("Dependency map");
            foreach (var source in map.Keys)
            {
                Dependency dependency = map[source];
                Console.WriteLine(string.Format("   Property {0} (type={1}) will update", dependency.source, dependency.source_type));
                foreach (var target in map[source].targets)
                    Console.WriteLine("      Property " + target);
            }
        }
    }
}
