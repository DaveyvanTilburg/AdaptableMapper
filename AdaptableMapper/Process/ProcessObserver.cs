namespace AdaptableMapper.Process
{
    public interface ProcessObserver
    {
        void InformationRaised(Information information);
    }
}