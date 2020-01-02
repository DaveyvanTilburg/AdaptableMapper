using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals.Xml
{
    public class XmlGetNumberOfHits : GetValueTraversal, SerializableByTypeId
    {
        public const string _typeId = "5f9d1ca5-95a8-4a07-aa44-4ea6e8ec8fa9";
        public string TypeId => _typeId;

        public XmlGetNumberOfHits() { }
        public XmlGetNumberOfHits(List<string> paths)
        {
            Paths = paths;
        }

        public List<string> Paths { get; set; }

        public string GetValue(Context context)
        {
            if (!(context.Source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XmlGetNumberOfHits#1; source is not of expected type XElement", "error", Paths, context.Source?.GetType().Name);
                return string.Empty;
            }

            int hits = 0;
            foreach(string path in Paths)
            {
                IEnumerable enumerable = null;

                try
                {
                    enumerable = xElement.XPathEvaluate(path) as IEnumerable;
                }
                catch(XPathException) 
                {
                    Process.ProcessObservable.GetInstance().Raise("XmlGetNumberOfHits#2; invalid path", "error", path);
                }

                if (enumerable is null)
                    continue;

                var xObjects = enumerable.Cast<XObject>();
                hits += xObjects.Count();
            }

            return hits.ToString();
        }
    }
}