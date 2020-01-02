using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public sealed class GetSearchValueTraversal : GetValueTraversal, SerializableByTypeId
    {
        public const string _typeId = "a077997f-3bee-4814-91d8-f88bf041005f";
        public string TypeId => _typeId;

        public GetSearchValueTraversal() { }
        public GetSearchValueTraversal(GetValueTraversal getValueTraversalSearchPath, GetValueTraversal getValueTraversalSearchValuePath)
        {
            GetValueTraversalSearchPath = getValueTraversalSearchPath;
            GetValueTraversalSearchValuePath = getValueTraversalSearchValuePath;
        }

        public GetValueTraversal GetValueTraversalSearchPath { get; set; }
        public GetValueTraversal GetValueTraversalSearchValuePath { get; set; }

        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            if(!(GetValueTraversalSearchPath is GetValueTraversalPathProperty pathProperty))
            {
                Process.ProcessObservable.GetInstance().Raise($"GetSearchValueTraversal#3; {nameof(GetValueTraversalSearchPath)} does not have a path to update with searchValue", "error");
                return string.Empty;
            }

            string searchValue = GetValueTraversalSearchValuePath.GetValue(context);

            string tempPath = pathProperty.Path;
            string actualPath = pathProperty.Path.Replace("{{searchValue}}", searchValue);
            pathProperty.Path = actualPath;

            string result = GetValueTraversalSearchPath.GetValue(context);
            pathProperty.Path = tempPath;

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversalSearchPath == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetSearchValueTraversal#1; {nameof(GetValueTraversalSearchPath)} cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalSearchValuePath == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetSearchValueTraversal#2; {nameof(GetValueTraversalSearchValuePath)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}