using System.Linq;
using Mono.Cecil;
using NotifyPropertyWeaver.Tasks;
using NLog;

namespace NotifyPropertyWeaver
{
    // Analysis steps
    // - Find classes to process
    // - - Find NotifyPropertyChanged method
    // - - Find dependencies
    // - - Find auto properties to instrument
    // - - Find collection auto properties to instrument
    // - - Find collection properties to generate notification methods for
    // - - Find "normal" properties to instrument
    // - - Find "normal" collection properties to instrument

    public class Analyzer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly AssemblyDefinition assembly;

        // Working data
        private MethodDefinition notify_property_changed_method;
        private readonly DependencyMap dependency_map = new DependencyMap();

        public Analyzer(AssemblyDefinition assembly)
        {
            this.assembly = assembly;
        }

        public void Execute()
        {
            log.Trace("Finding classes to process");
            var notify_property_classes = assembly.GetNotifyPropertyChangedClasses().ToList();
            foreach (var notify_property_class in notify_property_classes)
            {
                log.Trace("\tFound " + notify_property_class.Name);

                FindOrAddNotifyPropertyChangedMethod(notify_property_class);
                FindDependencies(notify_property_class);
                FindProperties(notify_property_class);
            }
        }

        private void FindOrAddNotifyPropertyChangedMethod(TypeDefinition notify_property_class)
        {
            log.Trace("\t\tFinding notify property changed method");

            notify_property_changed_method = notify_property_class.FindNotifyPropertyChangedMethod();
            if (notify_property_changed_method == null)
            {
                log.Trace("\t\t\tAdding NotifyPropertyChanged method");
                notify_property_changed_method = AddNotifyPropertyChangedMethodTask.Execute(notify_property_class);
            }
            else
                log.Trace("\t\t\tFound " + notify_property_changed_method.Name);
        }

        private void FindDependencies(TypeDefinition notify_property_class)
        {
            log.Trace("\t\tFinding property dependencies");

            dependency_map.Clear();

            DependencyAnalyzer dependency_analyzer = new DependencyAnalyzer();
            dependency_analyzer.Execute(notify_property_class, dependency_map);
        }

        private void FindProperties(TypeDefinition notify_property_class)
        {
            log.Trace("\t\tFinding properties");
            foreach (var property in notify_property_class.Properties)
            {
                log.Trace("\t\t\tProcessing " + property.Name);
                if (property.IsAutoProperty())
                {
                    if (property.IsCollectionAutoProperty() && dependency_map.HasDependencies(property.Name))
                    {
                        log.Trace("\t\t\t\tFound collection auto property");
                        MethodDefinition collection_notification_method = AddCollectionNotificationMethodTask.Execute(property, notify_property_changed_method, dependency_map);
                        AddNotificationsToAutoCollectionProperty.Execute(property, notify_property_changed_method, collection_notification_method);
                    }
                    else
                    {
                        log.Trace("\t\t\t\tFound auto property");
                        AddNotificationsToAutoPropertyTask.Execute(property, notify_property_changed_method, dependency_map);
                    }
                }
                else if (property.SetMethod != null)
                {
                    if (property.IsCollectionProperty() && dependency_map.HasDependencies(property.Name))
                    {
                        log.Trace("\t\t\t\tFound collection property");
                        MethodDefinition collection_notification_method = AddCollectionNotificationMethodTask.Execute(property, notify_property_changed_method, dependency_map);
                        AddNotificationsToNormalCollectionProperty.Execute(property, notify_property_changed_method, collection_notification_method);
                    }
                    else
                    {
                        log.Trace("\t\t\t\tFound property");
                        AddNotificationsToNormalPropertyTask.Execute(property, notify_property_changed_method, dependency_map);
                    }
                }
            }
        }
    }
}
