using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetTemplate : GetTemplate
    {
        public XmlGetTemplate(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template Get(object target)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#23; target is not of expected type XElement", "error", target?.GetType().Name, Path);
                return CreateNullTemplate();
            }

            XElement result = xElement.NavigateToPath(Path);
            if (result.NodeType == System.Xml.XmlNodeType.None)
                return CreateNullTemplate();

            if(result.Parent == null)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#26; template traversal did not result in an element that has a parent", "warning", Path);
                return CreateNullTemplate();
            }

            var template = new Template
            {
                Parent = result.Parent,
                Child = result
            };

            result.Remove();

            return template;
        }

        private static Template CreateNullTemplate()
        {
            return new Template { Parent = new XElement("nullObject"), Child = new XElement("nullObject") };
        }
    }
}