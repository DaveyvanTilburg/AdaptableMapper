using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetValueTraversal : GetValueTraversal, GetValueTraversalPathProperty, ResolvableByTypeId
    {
        public const string _typeId = "b3a8e531-2a44-42e3-bac4-1f6b0c1b80b3";
        public string TypeId => _typeId;

        public XmlGetValueTraversal() { }
        public XmlGetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public string GetValue(Context context)
        {
            if (!(context.Source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#17; source is not of expected type XElement", "error", Path, context.Source?.GetType().Name);
                return string.Empty;
            }

            MethodResult<string> result = xElement.GetXPathValue(Path.ConvertToInterpretation(XmlInterpretation));

            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}