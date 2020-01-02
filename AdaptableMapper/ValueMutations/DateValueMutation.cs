using System;
using System.Globalization;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class DateValueMutation : ValueMutation, SerializableByTypeId
    {
        public const string _typeId = "49c1f493-d14e-47cc-adef-43436898598c";
        public string TypeId => _typeId;

        public DateValueMutation() { }
        public DateValueMutation(string formatTemplate)
            => FormatTemplate = formatTemplate;

        public string FormatTemplate { get; set; }

        public string Mutate(Context context, string value)
        {
            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("DateValueMutation#1; value is not a valid date", "warning");
                return value;
            }

            string result;
            try
            {
                result = sourceDateTime.ToString(FormatTemplate);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DateValueMutation#2; value is not a valid date", "error", exception.Message, exception.GetType().Name);
                return value;
            }
            return result;
        }
    }
}