using System.Xml.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetThisValueTraversal : SetMutableValueTraversal, SerializableByTypeId
    {
        public const string _typeId = "0e33e050-8da6-4d87-ad49-ff9bde9bf953";
        public string TypeId => _typeId;

        public XmlSetThisValueTraversal() { }

        public bool SetAsCData { get; set; }

        protected override void SetValueImplementation(Context context, MappingCaches mappingCaches, string value)
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