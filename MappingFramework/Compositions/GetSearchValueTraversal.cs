using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public sealed class GetSearchValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
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
            if (!(GetValueTraversalSearchPath is GetValueTraversalPathProperty pathProperty))
            {
                context.InvalidType(GetValueTraversalSearchPath, typeof(GetValueTraversalPathProperty));
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

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversalSearchPath);
            visitor.Visit(GetValueTraversalSearchValuePath);
        }
    }
}