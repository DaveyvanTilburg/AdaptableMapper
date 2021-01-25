using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    [ContentType(ContentType.Any)]
    public sealed class GetSearchValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "a077997f-3bee-4814-91d8-f88bf041005f";
        public string TypeId => _typeId;

        public GetSearchValueTraversal() { }
        public GetSearchValueTraversal(GetSearchPathValueTraversal getValueTraversalSearchPath, GetValueTraversal getValueTraversalSearchValuePath)
        {
            GetValueTraversalSearchPath = getValueTraversalSearchPath;
            GetValueTraversalSearchValuePath = getValueTraversalSearchValuePath;
        }

        public GetSearchPathValueTraversal GetValueTraversalSearchPath { get; set; }
        public GetValueTraversal GetValueTraversalSearchValuePath { get; set; }

        public string GetValue(Context context)
        {
            string searchValue = GetValueTraversalSearchValuePath.GetValue(context);

            string tempPath = GetValueTraversalSearchPath.Path();
            string actualPath = tempPath.Replace("{{searchValue}}", searchValue);
            GetValueTraversalSearchPath.Path(actualPath);

            string result = GetValueTraversalSearchPath.GetValue(context);
            GetValueTraversalSearchPath.Path(tempPath);

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversalSearchPath);
            visitor.Visit(GetValueTraversalSearchValuePath);
        }
    }
}