using AdaptableMapper.Converters;

namespace AdaptableMapper.ValueMutations.Traversals
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

        public string GetValue(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                Process.ProcessObservable.GetInstance().Raise("SplitByCharTakePositionStringTraversal#1; source is empty", "warning", Separator, Position);
                return string.Empty;
            }

            int zeroBasedIndexPosition = Position - 1;
            string[] parts = source.Split(Separator);
            if (parts.Length < Position)
            {
                Process.ProcessObservable.GetInstance().Raise("SplitByCharTakePositionStringTraversal#2; split by char resulted in less parts than needed to take position", "warning", Separator, Position);
                return string.Empty;
            }

            return parts[zeroBasedIndexPosition];
        }
    }
}