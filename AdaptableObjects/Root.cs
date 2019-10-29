using System.Collections.Generic;
using AdaptableMapper.Memory.Language;

namespace AdaptableMapper
{
    public class Root : ModelBase
    {
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}