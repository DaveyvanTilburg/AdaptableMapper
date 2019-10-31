using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlTraversalTemplate : TraversalToGetTemplate
    {
        public XmlTraversalTemplate(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#23; target is not of expected type XElement", Path);
                return string.Empty;
            }

            XElement result = xElement.NavigateToPath(Path);
            if(result.Parent != null)
                result.Remove();

            return result;
        }
    }
}