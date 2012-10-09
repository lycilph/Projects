using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter
{
    public class DependencyMap
    {
        private readonly Dictionary<string, List<string>> dependency_map = new Dictionary<string, List<string>>();
        
        public void Add(string source, string target)
        {
            if (!dependency_map.ContainsKey(source))
                dependency_map.Add(source, new List<string>());

            if (!dependency_map[source].Contains(target))
                dependency_map[source].Add(target);
        }

        public IEnumerable<string> GetDependenciesFor(string source)
        {
            return (dependency_map.ContainsKey(source) ? dependency_map[source] : null);
        }

        public void Dump()
        {
            foreach (var source in dependency_map.Keys)
            {
                Console.WriteLine("Property " + source + " will update");
                foreach (var target in dependency_map[source])
                    Console.WriteLine("   Property " + target);
            }
        }
    }
}
