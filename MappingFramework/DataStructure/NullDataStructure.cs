namespace MappingFramework.DataStructure
{
    public sealed class NullDataStructure : TraversableDataStructure
    {
        internal override bool IsValid()
        {
            return false;
        }
    }
}