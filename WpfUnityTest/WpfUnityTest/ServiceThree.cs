namespace WpfUnityTest
{
    public class ServiceThree
    {
        private IServiceFour ServiceFour { get; set; }

        public ServiceThree(IServiceFour service_four)
        {
            ServiceFour = service_four;
        }

        public string Text
        {
            get { return "Service 3 - " + ServiceFour.Text; }
        }
    }
}
