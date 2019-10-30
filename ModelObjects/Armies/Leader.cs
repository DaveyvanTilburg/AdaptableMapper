using AdaptableMapper.Model.Language;

namespace ModelObjects.Armies
{
    public class Leader : ModelBase
    {
        public string Reference { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}