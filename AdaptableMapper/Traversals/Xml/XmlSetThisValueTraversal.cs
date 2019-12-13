using System.Xml.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetThisValueTraversal : SetValueTraversal
    {
        public bool SetAsCData { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#20; target is not of expected type XElement", "error", context.Target?.GetType().Name);
                return;
            }

            if (SetAsCData)
            {
                xElement.Add(new XCData(value));
            }
            else
            {
                xElement.Value = value;
            }
        }
    }
}