using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class GetAdditionalSourceValue : GetValueTraversal, ResolvableByTypeId, IVisitable
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

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversalForAdditionalSourceKey);
            visitor.Visit(GetValueTraversalForAdditionalSourceName);
        }
    }
}