using System;
using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Configuration.Xml
{
    public sealed class XmlTargetInstantiator : TargetInstantiator
    {
        public XmlInterpretation XmlInterpretation { get; set; }

        public XmlTargetInstantiator()
        {
            XmlInterpretation = XmlInterpretation.Default;
        }

        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#24; Source is not of expected type string", "error", source, source?.GetType().Name);
                return new XElement("nullObject");
            }

            XElement root;
            try
            {
                root = XElement.Parse(template);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#6; Template is not valid Xml", "error", exception.GetType().Name, exception.Message);
                return new XElement("nullObject");
            }

            if (XmlInterpretation == XmlInterpretation.WithoutNamespace)
                root.RemoveAllNamespaces();

            return root;
        }
    }
}