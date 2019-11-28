using System.Collections.Generic;
using AdaptableMapper.Model;

namespace ModelObjects.Simple
{
    public class DeepMix : ModelBase
    {
        public List<Mix> Mixes { get; set; } = new List<Mix>();
    }
}