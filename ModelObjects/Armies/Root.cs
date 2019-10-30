using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Armies
{
    public class Root : ModelBase
    {
        public List<Army> Armies { get; set; } = new List<Army>();
        public List<Leader> Leaders { get; set; } = new List<Leader>();
    }
}