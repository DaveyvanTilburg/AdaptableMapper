using System.IO;
using System.Xml;
using System.Xml.Linq;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Configuration.Xml
{
    [ContentType(ContentType.Xml)]
    public sealed class XElementToStringObjectConverter : ResultObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "37423a03-fdb3-4523-b94d-7ea1bd29f0b7";
        public string TypeId => _typeId;
        public bool UseIndentation { get; set; } = true;
        public bool IncludeDeclaration { get; set; } = true;

        public XElementToStringObjectConverter() { }

        public object Convert(object source)
        {
            XDocument xDocument = ((XElement)source).Document;

            using (StringWriter stringWriter = new StringWriter())
            {
                if (IncludeDeclaration)
                {
                    if (UseIndentation)
                        stringWriter.WriteLine(xDocument?.Declaration);
                    else
                        stringWriter.Write(xDocument?.Declaration);
                }

                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = UseIndentation }))
                    xDocument?.Save(xmlWriter);

                return stringWriter.ToString().Trim();
            }
        }
    }
}