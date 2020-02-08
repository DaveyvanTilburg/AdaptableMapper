namespace MappingFramework
{
    internal class NullMethodResult<T> : MethodResult<T>
    {
        public override bool IsValid => false;
    }
}