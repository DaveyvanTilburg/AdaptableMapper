using System;

namespace AdaptableMapper.Formats
{
    internal abstract class DateFormatTypeBase : FormatType
    {
        protected abstract string FormatString { get; }

        public override string Format(string source)
        {
            if (!DateTime.TryParse(source, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("Format#3; source is not a valid date", "warning", this.GetType().Name);
                return source;
            }

            return sourceDateTime.ToString(FormatString);
        }
    }
}