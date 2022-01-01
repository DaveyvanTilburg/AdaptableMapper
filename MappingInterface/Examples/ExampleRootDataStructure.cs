using MappingFramework.Languages.DataStructure;

namespace MappingFramework.MappingInterface.Examples
{
    internal class ExampleRootDataStructure : TraversableDataStructure
    {
        public ExampleRootDataStructure()
        {
            Children = new ChildList<ExampleChildDataStructure>(this);
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public ChildList<ExampleChildDataStructure> Children { get; set; }
    }
}