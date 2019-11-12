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
                Process.ProcessObservable.GetInstance().Raise("XML#23; target is not of expected type XElement", "error", target?.GetType().Name, Path);
                return string.Empty;
            }

            XElement result = xElement.NavigateToPath(Path);
            if(result.Parent == null)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#26; template traversal did not result in an element that has a parent", "error", Path);
                return string.Empty;
            }
            result.Remove();

            return result;
        }
    }
}