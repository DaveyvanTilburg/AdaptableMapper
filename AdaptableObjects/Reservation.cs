using System.Collections.Generic;
using AdaptableMapper.Memory.Language;

namespace AdaptableMapper
{
    public class Reservation : ModelBase
    {
        public string Id { get; set; } = string.Empty;

        public string HotelCode { get; set; } = string.Empty;

        public List<RoomStay> RoomStays { get; set; } = new List<RoomStay>();

        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}