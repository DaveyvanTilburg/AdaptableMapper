using System.Collections.Generic;
using AdaptableMapper;

namespace AdaptableMapper
{
    public class Root : Adaptable
    {
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}