using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetThisValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "2be460d7-4f86-4b72-983b-09b323d63abf";
        public string TypeId => _typeId;

        public XmlGetThisValueTraversal() { }

        public string GetValue(Context context)
        {
            if (!(context.Source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#16; source is not of expected type XElement", "error", context.Source?.GetType().Name);
                return string.Empty;
            }

            return xElement.Value;
        }
    }
}