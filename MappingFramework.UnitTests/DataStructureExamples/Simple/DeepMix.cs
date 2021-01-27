using MappingFramework.Languages.DataStructure;

namespace MappingFramework.TDD.DataStructureExamples.Simple
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