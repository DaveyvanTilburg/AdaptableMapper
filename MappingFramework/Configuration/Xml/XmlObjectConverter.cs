using System;
using System.IO;
using System.Xml.Linq;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Xml;

namespace MappingFramework.Configuration.Xml
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "d695068f-499b-4189-aaf6-4bb86d564889";
        public string TypeId => _typeId;

        public XmlObjectConverter()
        {
            XmlInterpretation = XmlInterpretation.Default;
        }

        public XmlInterpretation XmlInterpretation { get; set; }

        public object Convert(Context context, object source)
        {
            if (!(source is string input))
            {
                context.InvalidType(source, typeof(string));
                return NullElement.Create();
            }

            XElement root;
            try
            {
                var stringReader = new StringReader(input);
                var document = XDocument.Load(stringReader);
                root = document.Root;
            }
            catch(Exception exception)
            {
                context.OperationFailed(this, exception);
                root = NullElement.Create();
            }

            if(XmlInterpretation == XmlInterpretation.WithoutNamespace)
                root.RemoveAllNamespaces();

            return root;
        }
    }
}