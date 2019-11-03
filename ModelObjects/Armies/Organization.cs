using AdaptableMapper.Model.Language;

namespace ModelObjects.Armies
{
    public class Organization : ModelBase
    {
        public Organization()
        {
            Leaders = new ModelList<Leader>(this);
        }

        public ModelList<Leader> Leaders { get; set; }
    }
}