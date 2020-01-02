using System;
using System.IO;
using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Configuration.Xml
{
    public sealed class XmlTargetInstantiator : TargetInstantiator, SerializableByTypeId
    {
        public const string _typeId = "137a2d0b-c49f-491b-b5b9-24413f9969ee";
        public string TypeId => _typeId;

        public XmlTargetInstantiator()
        {
            XmlInterpretation = XmlInterpretation.Default;
        }

        public XmlInterpretation XmlInterpretation { get; set; }

        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#24; Source is not of expected type string", "error", source, source?.GetType().Name);
                return NullElement.Create();
            }

            XElement root;
            try
            {
                var stringReader = new StringReader(template);
                var document = XDocument.Load(stringReader);
                root = document.Root;
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#6; Template is not valid Xml", "error", exception.GetType().Name, exception.Message);
                return NullElement.Create();
            }

            if (XmlInterpretation == XmlInterpretation.WithoutNamespace)
                root.RemoveAllNamespaces();

            return root;
        }
    }
}