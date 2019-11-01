namespace AdaptableMapper.Errors
{
    public interface ProcessObserver
    {
        void InformationRaised(Information information);
    }
}