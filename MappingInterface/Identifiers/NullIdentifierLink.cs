namespace MappingFramework.MappingInterface.Identifiers
{
    public class NullIdentifierLink : IIdentifierLink
    {
        public NullIdentifierLink() { }
        
        public void SubscribeTo(Publisher<IdentifierLinkUpdateEventArgs> publisher) { }
        
        public void UnSubscribe(Publisher<IdentifierLinkUpdateEventArgs> publisher) { }
    }
}