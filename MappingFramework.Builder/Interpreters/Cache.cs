namespace MappingFramework.Builder.Interpreters
{
    internal class Cache : Interpreter
    {
        public string CommandName => "cache";

        public void Receive(Visitor visitor)
        {
            string cacheName = visitor.Command.Next();
            visitor.Stash(cacheName, visitor.Subject);

            visitor.Subject = null;
        }
    }
}