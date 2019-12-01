using AdaptableMapper.Formats;

namespace AdaptableMapper.Traversals
{
    public abstract class SetFormattedValueTraversal : SetValueTraversal
    {
        public Formatter Formatter { get; set; }

        protected SetFormattedValueTraversal(Formatter formatter)
            => Formatter = formatter;

        protected abstract void SetValueImplementation(object target, string value);

        public void SetValue(object target, string value)
        {
            string formattedValue = Formatter.Format(value);

            SetValueImplementation(target, formattedValue);
        }
    }
}