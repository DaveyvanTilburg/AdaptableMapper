using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetSearchValueTraversal : GetValueTraversal
    {
        public XmlGetSearchValueTraversal(string searchPath, string searchValuePath)
        {
            SearchPath = searchPath;
            SearchValuePath = searchValuePath;
        }

        public string SearchPath { get; set; }
        public string SearchValuePath { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public string GetValue(Context context)
        {
            if (!(context.Source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#13; source is not of expected type XElement", "error", SearchPath, SearchValuePath, context.Source?.GetType().Name);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SearchValuePath))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#25; SearchValuePath is empty. If this is intentional, please use XmlGetValue instead.", "error", SearchPath, SearchValuePath);
                return string.Empty;
            }

            MethodResult<string> searchValue = xElement.GetXPathValue(SearchValuePath.ConvertToInterpretation(XmlInterpretation));
            if (searchValue.IsValid && string.IsNullOrWhiteSpace(searchValue.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#14; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath);
                return string.Empty;
            }

            if (!searchValue.IsValid)
                return string.Empty;

            string actualPath = string.IsNullOrWhiteSpace(searchValue.Value) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue.Value);
            MethodResult<string> result = xElement.GetXPathValue(actualPath.ConvertToInterpretation(XmlInterpretation));

            return result.Value;
        }
    }
}