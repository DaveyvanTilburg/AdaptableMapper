namespace MappingFramework.Process
{
    public interface ProcessObserver
    {
        void InformationRaised(Information information);
    }
}