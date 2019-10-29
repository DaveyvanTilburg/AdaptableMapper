using AdaptableMapper.Model.Language;

namespace AdaptableMapper
{
    public class Guest : ModelBase
    {
        public string GuestId { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}