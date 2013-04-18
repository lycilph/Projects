using Microsoft.Practices.Unity;

namespace WpfUnityTest
{
    public class Controller : IController
    {
        public IServiceOne ServiceOne { get; set; }
        public IServiceTwo ServiceTwo { get; set; }
        public ServiceThree ServiceThree { get; set; }

        public Controller(IServiceOne service_one, IServiceTwo service_two, ServiceThree service_three)
        {
            ServiceOne = service_one;
            ServiceTwo = service_two;
            ServiceThree = service_three;
        }
    }
}