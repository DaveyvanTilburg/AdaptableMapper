namespace MappingFramework.Builder.Interpreters
{
    internal interface Interpreter
    {
        string CommandName { get; }
        void Receive(Visitor visitor);
    }
}