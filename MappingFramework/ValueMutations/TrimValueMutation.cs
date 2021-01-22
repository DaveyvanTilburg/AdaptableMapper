using MappingFramework.Configuration;
using MappingFramework.Converters;

namespace MappingFramework.ValueMutations
{
    public sealed class TrimValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "0613c12c-65c5-4f62-98a0-ab8ccdc5e592";
        public string TypeId => _typeId;

        public TrimValueMutation() { }
        public TrimValueMutation(string characters)
        {
            Characters = characters;
        }

        public string Characters { get; set; }

        public string Mutate(Context context, string value)
            => value.Trim((Characters ?? string.Empty).ToCharArray());
    }
}