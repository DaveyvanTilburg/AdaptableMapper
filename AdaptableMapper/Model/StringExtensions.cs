using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace AdaptableMapper.Model
{
    internal static class StringExtensions
    {
        public static bool TryGetObjectFilter(this string value, out ModelFilter filter)
        {
            filter = null;

            int positionStart = value.IndexOf('{');
            int positionEnd = value.LastIndexOf('}') + 1;
            if (positionStart == -1)
                return false;

            if (positionEnd == -1)
                return false;

            filter = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelFilter>(value.Substring(positionStart, positionEnd - positionStart));
            filter.ModelName = value.Substring(0, positionStart);
            return true;
        }

        public static Stack<string> ToStack(this string value)
        {
            return new Stack<string>(value.Split('/'));
        }

        public static Queue<string> ToQueue(this string value)
        {
            return new Queue<string>(value.Split('/'));
        }
    }
}