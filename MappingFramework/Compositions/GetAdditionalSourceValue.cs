using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Compositions
{
    public class GetAdditionalSourceValue : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "9b8e8006-318b-42a7-a521-5809cda750ce";
        public string TypeId => _typeId;

        public GetAdditionalSourceValue() { }
        public GetAdditionalSourceValue(GetValueTraversal getValueTraversalForAdditionalSourceName, GetValueTraversal getValueTraversalForAdditionalSourceKey)
        {
            GetValueTraversalForAdditionalSourceName = getValueTraversalForAdditionalSourceName;
            GetValueTraversalForAdditionalSourceKey = getValueTraversalForAdditionalSourceKey;
        }

        public GetValueTraversal GetValueTraversalForAdditionalSourceName { get; set; }
        public GetValueTraversal GetValueTraversalForAdditionalSourceKey { get; set; }

        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            string name = GetValueTraversalForAdditionalSourceName.GetValue(context);
            if (string.IsNullOrEmpty(name))
            {
                Process.ProcessObservable.GetInstance().Raise($"GetAdditionalSourceValue#3; {nameof(GetValueTraversalForAdditionalSourceName)} resulted in a empty value", "warning");
                return string.Empty;
            }


            string key = GetValueTraversalForAdditionalSourceKey.GetValue(context);
            if (string.IsNullOrWhiteSpace(key))
            {
                Process.ProcessObservable.GetInstance().Raise($"GetAdditionalSourceValue#4; {nameof(GetValueTraversalForAdditionalSourceKey)} resulted in a empty value", "warning");
                return string.Empty;
            }

            string result = context.AdditionalSourceValues.GetValue(name, key);
            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversalForAdditionalSourceName == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetAdditionalSourceValue#1; {nameof(GetValueTraversalForAdditionalSourceName)} cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalForAdditionalSourceKey == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetAdditionalSourceValue#2; {nameof(GetValueTraversalForAdditionalSourceKey)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}