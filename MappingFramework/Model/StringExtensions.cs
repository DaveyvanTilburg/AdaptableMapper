namespace MappingFramework.Model
{
    public static class StringExtensions
    {
        public static bool TryGetObjectFilter(this string value, out ModelFilter filter)
        {
            filter = null;

            int positionStart = value.IndexOf('{');
            int positionEnd = value.LastIndexOf('}');
            if (positionStart == -1)
                return false;
                

            if (positionEnd == -1)
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#32; No } found in after {", "error", value);
                return false;
            }

            filter = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelFilter>(value.Substring(positionStart, positionEnd + 1 - positionStart));
            filter.ModelName = value.Substring(0, positionStart);
            return true;
        }
    }
}