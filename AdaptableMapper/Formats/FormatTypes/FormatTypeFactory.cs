using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Formats.FormatTypes
{
    internal partial class FormatTypeFactory
    {
        private readonly IReadOnlyCollection<FormatType> _formatTypes;

        public FormatTypeFactory()
        {
            _formatTypes = new List<FormatType>
            {
                new NullFormatType(),
                new DateFormatType(),
                new Decimal2FormatType()
            };
        }

        public IReadOnlyCollection<string> GetAllKeys()
            => _formatTypes.Select(f => f.Key).ToList();

        public FormatType GetFormatType(string key)
            => _formatTypes.FirstOrDefault(f => f.Key.Equals(key, System.StringComparison.InvariantCultureIgnoreCase)); 
    }
}