namespace MappingFramework.Languages.DataStructure
{
    public sealed class NullDataStructure : TraversableDataStructure
    {
        internal override bool IsValid()
        {
            return false;
        }
    }
}