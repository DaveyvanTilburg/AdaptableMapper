using MappingFramework.Model;

namespace ModelObjects.Simple
{
    public class DeepMix : ModelBase
    {
        public DeepMix()
        {
            Mixes = new ModelList<Mix>(this);
        }

        public ModelList<Mix> Mixes { get; set; }
    }
}