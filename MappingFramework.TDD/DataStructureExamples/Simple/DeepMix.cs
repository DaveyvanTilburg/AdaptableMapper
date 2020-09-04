using MappingFramework.DataStructure;

namespace MappingFramework.TDD.Simple
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