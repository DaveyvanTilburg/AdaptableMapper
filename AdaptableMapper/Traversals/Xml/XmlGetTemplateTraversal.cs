using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetTemplateTraversal : GetTemplateTraversal
    {
        public XmlGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public Template Get(object target)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#23; target is not of expected type XElement", "error", target?.GetType().Name, Path);
                return CreateNullTemplate();
            }

            XElement result = xElement.NavigateToPath(Path.ConvertToInterpretation(XmlInterpretation));
            if (result.NodeType == System.Xml.XmlNodeType.None)
                return CreateNullTemplate();

            if(result.Parent == null)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#26; template traversal did not result in an element that has a parent", "error", Path);
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
            => new Template { Parent = new XElement("nullObject"), Child = new XElement("nullObject") };
    }
}