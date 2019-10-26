using AdaptableMapper;

namespace AdaptableMapper
{
    public class Guest : Adaptable
    {
        public string GuestId { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
    }
}