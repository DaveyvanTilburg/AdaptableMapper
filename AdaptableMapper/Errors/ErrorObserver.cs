namespace AdaptableMapper.Errors
{
    public interface ErrorObserver
    {
        void ErrorOccured(Error error);
    }
}