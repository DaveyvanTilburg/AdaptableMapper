using System;
using System.Globalization;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class DateValueMutation : ValueMutation
    {
        public string FormatTemplate { get; set; }

        public DateValueMutation(string formatTemplate)
            => FormatTemplate = formatTemplate;

        public string Mutate(Context context, string source)
        {
            if (!DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime sourceDateTime))
            {
                Process.ProcessObservable.GetInstance().Raise("DateValueMutation#1; source is not a valid date", "warning");
                return source;
            }

            string result;
            try
            {
                result = sourceDateTime.ToString(FormatTemplate);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("DateValueMutation#2; source is not a valid date", "error", exception.Message, exception.GetType().Name);
                return source;
            }
            return result;
        }
    }
}