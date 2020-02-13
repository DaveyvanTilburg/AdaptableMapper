using System;
using System.Globalization;
using MappingFramework.Configuration;
using MappingFramework.Converters;

namespace MappingFramework.ValueMutations
{
    public sealed class DateValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "49c1f493-d14e-47cc-adef-43436898598c";
        public string TypeId => _typeId;

        public DateValueMutation() { }

        public string ReadFormatTemplate { get; set; }
        public string FormatTemplate { get; set; }

        public string Mutate(Context context, string value)
        {
            DateTime source;
            if (string.IsNullOrWhiteSpace(ReadFormatTemplate))
            {
                if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out source))
                {
                    Process.ProcessObservable.GetInstance().Raise("DateValueMutation#1; value is not a date that is directly interpretable", "warning");
                    return value;
                }
            }
            else
            {
                if (!DateTime.TryParseExact(value, ReadFormatTemplate, CultureInfo.InvariantCulture, DateTimeStyles.None, out source))
                {
                    Process.ProcessObservable.GetInstance().Raise($"DateValueMutation#3; value {value} does not match format {ReadFormatTemplate}", "warning");
                    return value;
                }
            }


            string result;
            try
            {
                result = source.ToString(string.IsNullOrWhiteSpace(FormatTemplate) ? "s" : FormatTemplate);
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DateValueMutation#2; value is not a valid date", "error", exception.Message, exception.GetType().Name);
                return value;
            }
            return result;
        }
    }
}