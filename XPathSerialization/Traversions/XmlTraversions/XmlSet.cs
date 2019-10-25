﻿using System.Xml.Linq;

namespace XPathSerialization.Traversions.XmlTraversions
{
    public class XmlSet : SetTraversion
    {
        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return;
            }

            xElement.SetXPathValues(Path, value);
        }
    }
}