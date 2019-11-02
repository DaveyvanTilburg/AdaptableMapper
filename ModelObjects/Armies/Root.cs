using AdaptableMapper.Model.Language;

namespace ModelObjects.Armies
{
    public class Root : ModelBase
    {
        public Root()
        {
            Armies = new ModelList<Army>(this);
            Leaders = new ModelList<Leader>(this);
        }

        public ModelList<Army> Armies { get; set; }
        public ModelList<Leader> Leaders { get; set; }
    }
}