namespace MappingFramework.MappingInterface.Identifiers
{
    public interface IIdentifierLink
    {
        void SubscribeTo(Publisher<IdentifierLinkUpdateEventArgs> publisher);

        void UnSubscribe(Publisher<IdentifierLinkUpdateEventArgs> publisher);
    }
}