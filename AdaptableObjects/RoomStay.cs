﻿using AdaptableMapper.Memory.Language;

namespace AdaptableMapper
{
    public class RoomStay : ModelBase
    {
        public string Code { get; set; } = string.Empty;
        public string GuestId { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string RateCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}