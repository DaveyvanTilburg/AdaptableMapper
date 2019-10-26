using System.Collections.Generic;

namespace AdaptableMapper
{
    internal static class StringExtensions
    {
        public static bool TryGetObjectFilter(this string value, out AdaptableFilter filter)
        {
            filter = null;

            int positionStart = value.IndexOf('{');
            int positionEnd = value.LastIndexOf('}') + 1;
            if (positionStart == -1)
                return false;

            if (positionEnd == -1)
                return false;

            filter = Newtonsoft.Json.JsonConvert.DeserializeObject<AdaptableFilter>(value.Substring(positionStart, positionEnd - positionStart));
            filter.AdaptableName = value.Substring(0, positionStart);
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