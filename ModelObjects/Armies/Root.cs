using AdaptableMapper.Model.Language;

namespace ModelObjects.Armies
{
    public class Root : ModelBase
    {
        public Root()
        {
            Armies = new ModelList<Army>(this);
        }

        public ModelList<Army> Armies { get; set; }
        public Organization Organization { get; set; } = new Organization();
    }
}