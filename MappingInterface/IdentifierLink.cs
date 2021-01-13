namespace MappingFramework.MappingInterface
{
    public class IdentifierLink
    {
        public delegate void IdentifierUpdate(string text);
        private object _subscribedTo;

        private readonly IdentifierUpdate _identifierUpdate;
        
        public IdentifierLink(IdentifierUpdate identifierUpdate)
        {
            _identifierUpdate = identifierUpdate;
        }
        
        public void SubscribeTo(Publisher<IdentifierLinkUpdateEventArgs> publisher)
        {
            if(_subscribedTo == null)
            {
                publisher.UpdateEvent += OnUpdateEvent;
                _subscribedTo = publisher;
            }
        }
        
        public void UnSubscribe(Publisher<IdentifierLinkUpdateEventArgs> publisher)
        {
            if(_subscribedTo == publisher)
            {
                _subscribedTo = null;
                _identifierUpdate?.Invoke(string.Empty);
            }
        }
        
        private void OnUpdateEvent(object sender, IdentifierLinkUpdateEventArgs eventArgs)
        {
            if(_subscribedTo == sender)
                _identifierUpdate?.Invoke(eventArgs.Text);
        }
    }
}