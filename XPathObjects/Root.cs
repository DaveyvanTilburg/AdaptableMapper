using System.Collections.Generic;
using XPathSerialization;

namespace XPathObjects
{
    public class Root : Adaptable
    {
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}