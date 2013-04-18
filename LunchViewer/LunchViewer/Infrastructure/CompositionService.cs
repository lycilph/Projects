using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace LunchViewer.Infrastructure
{
    public class CompositionService
    {
        private static readonly CompositionService instance = new CompositionService();

        private CompositionService()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            Container = new CompositionContainer(catalog);
        }

        public static CompositionService Instance
        {
            get { return instance; }
        }

        public static void Compose(object obj)
        {
            Instance.Container.ComposeParts(obj);
        }

        public CompositionContainer Container { get; private set; }
    }
}
