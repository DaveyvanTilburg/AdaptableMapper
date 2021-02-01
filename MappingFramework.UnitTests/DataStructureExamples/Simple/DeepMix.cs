using MappingFramework.Languages.DataStructure;

namespace MappingFramework.UnitTests.DataStructureExamples.Simple
{
    public class DeepMix : TraversableDataStructure
    {
        public DeepMix()
        {
            Mixes = new ChildList<Mix>(this);
        }

        public ChildList<Mix> Mixes { get; set; }
    }
}