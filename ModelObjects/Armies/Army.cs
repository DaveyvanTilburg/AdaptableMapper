using AdaptableMapper.Model.Language;

namespace ModelObjects.Armies
{
    public class Army : ModelBase
    {
        public Army()
        {
            Platoons = new ModelList<Platoon>(this);
        }

        public ModelList<Platoon> Platoons { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}