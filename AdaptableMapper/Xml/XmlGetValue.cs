using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetValue : GetValueTraversal
    {
        public XmlGetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ProcessObservable.GetInstance().Raise("XML#17; source is not of expected type XElement", "error", Path, source?.GetType()?.Name);
                return string.Empty;
            }

            return xElement.GetXPathValue(Path);
        }
    }
}