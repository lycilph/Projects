using System.Collections.Generic;

namespace NotifyPropertyWeaver
{
    public class DependencyMap
    {
        private readonly Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();
        
        public void Add(string source, string target)
        {
            if (!map.ContainsKey(source))
                map.Add(source, new List<string>());

            if (!map[source].Contains(target))
                map[source].Add(target);
        }

        public void Clear()
        {
            map.Clear();
        }

        public IEnumerable<string> GetDependenciesFor(string source)
        {
            return (map.ContainsKey(source) ? map[source] : new List<string>());
        }

        public bool HasDependencies(string source)
        {
            return (map.ContainsKey(source) && map[source].Count > 0);
        }
    }
}
