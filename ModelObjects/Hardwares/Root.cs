using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace ModelObjects.Hardwares
{
    public class Root : ModelBase
    {
        public List<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
    }
}