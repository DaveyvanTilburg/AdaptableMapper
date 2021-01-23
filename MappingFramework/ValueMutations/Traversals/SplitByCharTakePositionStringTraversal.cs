using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Process;

namespace MappingFramework.ValueMutations.Traversals
{
    public sealed class SplitByCharTakePositionStringTraversal : GetValueStringTraversal, ResolvableByTypeId
    {
        public const string _typeId = "b493ac95-5a3c-44e7-9042-440905bd5b21";
        public string TypeId => _typeId;

        public SplitByCharTakePositionStringTraversal() { }
        public SplitByCharTakePositionStringTraversal(char separator, int position)
        {
            Separator = separator;
            Position = position;
        }

        public char Separator { get; set; }
        public int Position { get; set; }

        public string GetValue(Context context, string source)
        {
            int zeroBasedIndexPosition = Position - 1;
            string[] parts = source.Split(Separator);
            if (parts.Length < Position)
            {
                context.AddInformation("Split by char resulted in less parts than needed to take position", InformationType.Warning);
                return string.Empty;
            }

            return parts[zeroBasedIndexPosition];
        }
    }
}