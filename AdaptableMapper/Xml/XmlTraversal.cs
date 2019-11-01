using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlTraversal : Traversal
    {
        public XmlTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#22; target is not of expected type XElement", Path, target?.GetType()?.Name);
                return string.Empty;
            }

            XElement result = xElement.NavigateToPath(Path);
            return result;
        }
    }
}