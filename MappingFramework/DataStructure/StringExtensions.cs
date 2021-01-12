using MappingFramework.Process;

namespace MappingFramework.DataStructure
{
    public static class StringExtensions
    {
        public static bool TryGetObjectFilter(this string value, out DataStructureFilter filter)
        {
            filter = null;

            int positionStart = value.IndexOf('{');
            int positionEnd = value.LastIndexOf('}');
            if (positionStart == -1)
                return false;
                

            if (positionEnd == -1)
            {
                ProcessObservable.GetInstance().Raise("DataStructure#32; No } found in after {", "error", value);
                return false;
            }

            filter = Newtonsoft.Json.JsonConvert.DeserializeObject<DataStructureFilter>(value.Substring(positionStart, positionEnd + 1 - positionStart));
            filter.DataStructureName = value.Substring(0, positionStart);
            return true;
        }
    }
}