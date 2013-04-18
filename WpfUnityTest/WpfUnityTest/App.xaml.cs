using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Unity;

namespace WpfUnityTest
{
    public partial class App
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();

            var types = Assembly.GetExecutingAssembly().GetTypes();
            var interfaces = types.Where(t => t.IsInterface).ToList();
            foreach (var interface_type in interfaces)
            {
                var name_parts = interface_type.FullName.Split(new char[] {'.'});
                name_parts[name_parts.Length - 1] = name_parts[name_parts.Length - 1].Substring(1);
                var implementation_name = string.Join(".", name_parts);
                var implementation_type = Assembly.GetExecutingAssembly().GetType(implementation_name);

                if (implementation_type != null)
                    container.RegisterType(interface_type, implementation_type);
            }

            var controller = container.Resolve<IController>();
        }
    }
}
