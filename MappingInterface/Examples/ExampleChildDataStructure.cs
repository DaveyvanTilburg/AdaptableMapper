using MappingFramework.Languages.DataStructure;

namespace MappingFramework.MappingInterface.Examples
{
    internal class ExampleChildDataStructure : TraversableDataStructure
    {
        public ExampleChildDataStructure()
        {
            Children = new ChildList<ExampleChildDataStructure>(this);
        }
        public ChildList<ExampleChildDataStructure> Children { get; set; }
        public string Content { get; set; }
    }
}