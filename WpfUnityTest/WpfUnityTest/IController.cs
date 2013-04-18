namespace WpfUnityTest
{
    public interface IController
    {
        IServiceOne ServiceOne { get; set; }
        IServiceTwo ServiceTwo { get; set; }
        ServiceThree ServiceThree { get; set; }
    }
}
