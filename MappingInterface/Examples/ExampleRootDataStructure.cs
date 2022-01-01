using MappingFramework.Languages.DataStructure;

namespace MappingFramework.MappingInterface.Examples
{
    internal class ExampleRootDataStructure : TraversableDataStructure
    {
        public ExampleRootDataStructure()
        {
            Children = new ChildList<ExampleChildDataStructure>(this);
            SubStructure = new ExampleRootSubStructure();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public ChildList<ExampleChildDataStructure> Children { get; set; }
        public ExampleRootSubStructure SubStructure { get; set; }
    }

    internal class ExampleRootSubStructure : TraversableDataStructure
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}