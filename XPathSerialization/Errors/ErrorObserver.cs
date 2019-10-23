namespace XPathSerialization.Errors
{
    public interface ErrorObserver
    {
        void ErrorOccured(Error error);
    }
}