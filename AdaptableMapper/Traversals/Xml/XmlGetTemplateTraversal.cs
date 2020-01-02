using System.Xml.Linq;
using AdaptableMapper.Converters;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetTemplateTraversal : GetTemplateTraversal, ResolvableByTypeId
    {
        public const string _typeId = "f6459be3-8ff7-438a-bcee-832d17be9af0";
        public string TypeId => _typeId;

        public XmlGetTemplateTraversal() { }
        public XmlGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public Template GetTemplate(object target, MappingCaches mappingCaches)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#23; target is not of expected type XElement", "error", target?.GetType().Name, Path);
                return CreateNullTemplate();
            }

            XElement result = xElement.NavigateToPath(Path.ConvertToInterpretation(XmlInterpretation));
            if (result.NodeType == System.Xml.XmlNodeType.None)
                return CreateNullTemplate();

            var template = new Template
            {
                Parent = result.Parent
            };

            var templateCache = mappingCaches.GetCache<TemplateCache>(nameof(TemplateCache));

            bool hasAccessed = templateCache.HasAccessed(Path, target);
            object storedTemplate = templateCache.GetTemplate(Path, target);

            if (storedTemplate == null)
            {
                templateCache.SetTemplate(Path, result);
                result.Remove();

                storedTemplate = result;
            }
            else if (!hasAccessed)
            {
                result.Remove();
            }

            template.Child = storedTemplate;

            return template;
        }

        private static Template CreateNullTemplate()
            => new Template { Parent = NullElement.Create(), Child = NullElement.Create() };
    }
}