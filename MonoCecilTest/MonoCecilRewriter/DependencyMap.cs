using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter
{
    public class DependencyMap
    {
        private readonly Dictionary<string, List<string>> property_dependency_map = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<string>> collection_dependency_map = new Dictionary<string, List<string>>();
        
        public void AddPropertyDependency(string source, string target)
        {
            if (!property_dependency_map.ContainsKey(source))
                property_dependency_map.Add(source, new List<string>());

            if (!property_dependency_map[source].Contains(target))
                property_dependency_map[source].Add(target);
        }

        public void AddCollectionDependency(string source, string target)
        {
            if (!collection_dependency_map.ContainsKey(source))
                collection_dependency_map.Add(source, new List<string>());

            if (!collection_dependency_map[source].Contains(target))
                collection_dependency_map[source].Add(target);
        }

        public IEnumerable<string> GetPropertyDependenciesFor(string source)
        {
            return (property_dependency_map.ContainsKey(source) ? property_dependency_map[source] : new List<string>());
        }

        public IEnumerable<string> GetCollectionDependenciesFor(string source)
        {
            return (collection_dependency_map.ContainsKey(source) ? collection_dependency_map[source] : new List<string>());
        }

        public IEnumerable<string> GetCollectionSources()
        {
            return (collection_dependency_map.Keys.Count > 0 ? new List<string>(collection_dependency_map.Keys) : new List<string>());
        }

        public void Dump()
        {
            Console.WriteLine("Dependency map");
            foreach (var source in property_dependency_map.Keys)
            {
                Console.WriteLine("   Property " + source + " will update");
                foreach (var target in property_dependency_map[source])
                    Console.WriteLine("      Property " + target);
            }
            foreach (var source in collection_dependency_map.Keys)
            {
                Console.WriteLine("   Collection " + source + " will update");
                foreach (var target in collection_dependency_map[source])
                    Console.WriteLine("      Property " + target);
            }
        }
    }
}
