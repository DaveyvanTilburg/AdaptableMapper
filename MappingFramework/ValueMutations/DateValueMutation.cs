using System;
using System.Globalization;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Process;

namespace MappingFramework.ValueMutations
{
    [ContentType(ContentType.Any)]
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
                    context.AddInformation($"Value: {value}, is not a valid date that is directly interpretable", InformationType.Warning);
                    return value;
                }
            }
            else
            {
                if (!DateTime.TryParseExact(value, ReadFormatTemplate, CultureInfo.InvariantCulture, DateTimeStyles.None, out source))
                {
                    context.AddInformation($"Value: {value}, is not a valid date that is interpretable with the format: {ReadFormatTemplate}", InformationType.Warning);
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
                context.AddInformation($"Format: {FormatTemplate} resulted in an exception", InformationType.Warning, exception);
                return value;
            }
            return result;
        }
    }
}