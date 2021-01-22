namespace MappingFramework.Visitors
{
    internal interface IVisitable
    {
        void Receive(IVisitor visitor);
    }
}