using System.Windows;
using Roslyn.Compilers;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace CategorizationEngine
{
    public class ScriptHost
    {
        private readonly ScriptEngine script_engine;
        private readonly Session script_session;

        public Profile Profile { get; private set; }
        public MainWindowViewModel ViewModel { get; private set; }
        public CategoryController CategoryController { get; private set; }

        public ScriptHost(Profile profile, CategoryController category_controller, MainWindowViewModel vm)
        {
            script_engine = new ScriptEngine();
            script_session = script_engine.CreateSession(this);
            script_session.AddReference("System");
            script_session.AddReference(GetType().Assembly.Location);

            Profile = profile;
            ViewModel = vm;
            CategoryController = category_controller;
        }

        public bool Execute(string command)
        {
            try
            {
                script_session.Execute(command);
                return true;
            }
            catch (CompilationErrorException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        // Shortcuts for use by the gui
        public void Categorize()
        {
            ViewModel.Categorize();
        }

        public void GenerateGraphData()
        {
            ViewModel.GenerateGraphData();
        }

        public void NewProfile()
        {
            ViewModel.Initialize();
        }

        public void RandomGraph()
        {
            ViewModel.RandomGraph();
        }

        public void ByYear(string pattern)
        {
            Category category = CategoryController.Find(pattern);
            ViewModel.CategoryByYearGraph(category);
        }
    }
}
